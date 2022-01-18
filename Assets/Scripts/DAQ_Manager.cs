using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using LSL;

public class DAQ_Manager : MonoBehaviour
{
    private static string DAQ_Output = "";

    private static string last_flag;

    private static float time_since_last;
    private static bool started;
    private static bool in_activity;

    const string FLAG_START_SEQUENCE = "S"; // when press space/r-trigger
    const string FLAG_IDLE = "I"; // countdowns, movements, aiming, anything non-task/non-rest (must be after a non-S flag)
    const string FLAG_ANKLE = "B"; // start of task - move ankle for 5 seconds
    const string FLAG_HAND = "R"; // start of rest - squeeze ball for 5 seconds
    const string FLAG_END_SEQUENCE = "E";

    const float LSL_START_SEQUENCE = 100; // when press space/r-trigger
    const float LSL_IDLE = 101; // countdowns, movements, aiming, anything non-task/non-rest (must be after a non-S flag)
    const float LSL_ANKLE = 102; // start of task - move ankle for 5 seconds
    const float LSL_HAND = 103; // start of rest - squeeze ball for 5 seconds
    const float LSL_END_SEQUENCE = 104;

    private static StreamOutlet outlet;
    private static float[] currentSample;
    private static float prevTimeLSL = 0;
    private static float curTimeLSL;

    public static string StreamName = "Unity.LEBCI_Stream";
    public static string StreamType = "Unity.StreamType";
    public static string StreamID = "LE_BCI";

    // Start is called before the first frame update
    void Start()
    {
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 2, 0.0, LSL.channel_format_t.cf_float32);
        XMLElement chans = streamInfo.desc().append_child("channels");
        chans.append_child("channel").append_child_value("label", "Marker");
        chans.append_child("channel").append_child_value("label", "Time_manual");
        outlet = new StreamOutlet(streamInfo);
        currentSample = new float[2];

        DAQ_Output = "";

        started = false;
        in_activity = false;

        time_since_last = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (started && in_activity)
        {
            time_since_last += Time.deltaTime;
        }
    }

    public static void setFlag(string flag)
    {
        in_activity = false;

        // error checking:

        if (flag != FLAG_START_SEQUENCE &&
            flag != FLAG_IDLE &&
            flag != FLAG_ANKLE &&
            flag != FLAG_HAND &&
            flag != FLAG_END_SEQUENCE)
        {
            print("ERROR - Invalid flag code received");
            return;
        }

        // if start

        if (flag.Equals(FLAG_START_SEQUENCE))
        {
            // log start time

            System.DateTime dateTime = System.DateTime.Now;

            DAQ_Output += dateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + "\n";

            // reset time
            
            time_since_last = 0;

            // add start flag

            DAQ_Output += flag;

            updateCurrentSample(flag);

            // start sequence

            started = true;

            // log

            updateDAQLog();
        }

        // if started, and flag != last_flag

        else if (!flag.Equals(last_flag))
        {
            // add time

            DAQ_Output += time_since_last.ToString("0.000000");
            DAQ_Output += "\n";

            time_since_last = 0;

            // add flag

            DAQ_Output += flag;

            updateCurrentSample(flag);

            // log

            updateDAQLog();
        }

        // continue

        if (flag.Equals(FLAG_END_SEQUENCE))
        {
            DAQ_Output += "\n";

            last_flag = flag;
            started = false;
            in_activity = false;
        }
        else
        {
            last_flag = flag;
            in_activity = true;
        }
    }

    private static void updateDAQLog()
    {
        try
        {
            System.IO.File.WriteAllText(@"C:\Users\chava\OneDrive\Desktop\lebci_log.txt", DAQ_Output);
        }
        catch(Exception e)
        {
            print("Saving unavailable... \nDAQ: " + DAQ_Output);
        }

        //outlet.push_sample(currentSample);
    }

    private static void updateCurrentSample(string marker)
    {
        if (marker.Equals(FLAG_START_SEQUENCE))
        {
            currentSample[0] = LSL_START_SEQUENCE;
        }
        else if (marker.Equals(FLAG_IDLE))
        {
            currentSample[0] = LSL_IDLE;
        }
        else if(marker.Equals(FLAG_ANKLE))
        {
            currentSample[0] = LSL_ANKLE;
        }
        else if (marker.Equals(FLAG_HAND))
        {
            currentSample[0] = LSL_HAND;
        }
        else if (marker.Equals(FLAG_END_SEQUENCE))
        {
            currentSample[0] = LSL_END_SEQUENCE;
        }

        double timestamp = Stopwatch.GetTimestamp();
        curTimeLSL = (float) (timestamp / Stopwatch.Frequency);

        if (prevTimeLSL == 0)
        {
            currentSample[1] = 0;

            prevTimeLSL = curTimeLSL;
        }
        else
        {
            float diff = curTimeLSL - prevTimeLSL;

            currentSample[1] = diff;

            prevTimeLSL = curTimeLSL;
        }

        outlet.push_sample(currentSample);
    }
}