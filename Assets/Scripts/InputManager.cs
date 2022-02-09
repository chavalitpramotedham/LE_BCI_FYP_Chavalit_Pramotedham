using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using LSL;

public class InputManager : MonoBehaviour
{
    public bool dataCollectionMode;
    private bool actionDetected = false;
    public bool isListening = false;

    private InputDevice targetDevice;

    public string StreamType = "DL";

    StreamInfo[] streamInfos;
    StreamInlet streamInlet;

    float[] sample;
    private int channelCount;

    private void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }
    // Update is called once per frame
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);

        if (dataCollectionMode)
        {
            actionDetected = true;
        }

        if (triggerValue > 0.1f && isListening)
        {
            Debug.Log("trigger pressed: " + triggerValue);
            actionDetected = true;
        }

        if (isListening){
            if (streamInlet == null){
                streamInfos = LSL.resolve_stream("type", StreamType, 1, 0.0);

                if (streamInfos.Length > 0)
                {
                    streamInlet = new StreamInlet(streamInfos[0]);
                    channelCount = streamInlet.info().channel_count();
                    streamInlet.open_stream();
                }
            }

            if (streamInlet != null)
            {
                sample = new float[channelCount];
                double lastTimeStamp = streamInlet.pull_sample(sample, 0.0f);

                if (lastTimeStamp != 0.0)
                {
                    processInletSample(sample[0]);
                }
            }
        }

        if (Input.GetKeyDown("space") && isListening)
        {
            actionDetected = true;
        }
    }

    private void processInletSample(float sample){
        if (sample == 0.0){
            actionDetected = false;
        }
        else if (sample == 1.0){
            actionDetected = true;
        }
        print("ACTION DETECTED: " + actionDetected);

        isListening = false;
    }

    public void startListening(float duration)
    {
        actionDetected = false;
        isListening = true;
        // StartCoroutine(Listen(duration));
    }


    private IEnumerator Listen(float duration)
    {
        isListening = true;

        float normalizedTime = 1f;

        while (normalizedTime >= 0f)
        {
            normalizedTime -= Time.deltaTime / duration;
            yield return null;
        }

        isListening = false;
    }

    public bool getActionDetected()
    {
        return actionDetected;
    }
}
