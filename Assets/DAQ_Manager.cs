using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAQ_Manager : MonoBehaviour
{
    private static string DAQ_Output = "";

    private static string last_flag;

    private static float time_since_last;
    private static bool started;
    private static bool in_activity;

    const string FLAG_START_SEQUENCE = "S"; // when press space/r-trigger
    const string FLAG_IDLE = "I"; // countdowns, movements, aiming, anything non-task/non-rest (must be after a non-S flag)
    const string FLAG_TASK_ANKLE = "B"; // start of task - move ankle for 5 seconds
    const string FLAG_REST_HAND = "R"; // start of rest - squeeze ball for 5 seconds
    const string FLAG_END_SEQUENCE = "E";

    // Start is called before the first frame update
    static void Start()
    {
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
            flag != FLAG_TASK_ANKLE &&
            flag != FLAG_REST_HAND &&
            flag != FLAG_END_SEQUENCE)
        {
            print("ERROR - Invalid flag code received");
            return;
        }

        // if start

        if (flag.Equals(FLAG_START_SEQUENCE))
        {
            // log start time

            DAQ_Output += " " + System.DateTime.Now.ToString() + " ";

            // reset time
            
            time_since_last = 0;

            // add start flag

            DAQ_Output += flag;

            // start sequence

            started = true;

            // log

            print("DAQ: " + DAQ_Output);
        }

        // if started, and flag != last_flag

        else if (!flag.Equals(last_flag))
        {
            // add time

            DAQ_Output += time_since_last.ToString("0.00");
            time_since_last = 0;

            // add flag

            DAQ_Output += flag;

            // log

            print("DAQ: " + DAQ_Output);
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
}
