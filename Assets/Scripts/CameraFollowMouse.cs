using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private float yaw_min = -19.5f;
    private float yaw_max = 19.5f;

    private float pitch_min = -13f;
    private float pitch_max = 20f;
    

    // Update is called once per frame
    void Update()
    {
        if (yaw <= yaw_max && yaw >= yaw_min)
        {
            yaw += speedH * Input.GetAxis("Mouse X");

            if (yaw > yaw_max)
            {
                yaw = yaw_max;
            }
            else if (yaw < yaw_min)
            {
                yaw = yaw_min;
            }
        }
        if (pitch <= pitch_max && pitch >= pitch_min)
        {
            pitch -= speedV * Input.GetAxis("Mouse Y");

            if (pitch > pitch_max)
            {
                pitch = pitch_max;
            }
            else if (pitch < pitch_min)
            {
                pitch = pitch_min;
            }
        }

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}