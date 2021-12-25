using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAQ_Manager : MonoBehaviour
{
    private string output;

    private float time_since_last;
    private bool started;
    private bool in_activity;

    // Start is called before the first frame update
    void Start()
    {
        output = "";

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

    public void setFlag(int flag_number)
    {
        // error checking:

        if (flag_number > 1) // incomplete!:
        {
            in_activity = false;

            if (!started)
            {
                // put start flag first
                started = true;
            }
            else
            {
                //put previous activity time
                //reset activity time = 0
            }

            // put new activity flag

            in_activity = true;
        }
    }
}
