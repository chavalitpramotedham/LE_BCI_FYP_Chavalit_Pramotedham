using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    public bool dataCollectionMode;
    private bool actionDetected = false;
    private bool isListening = false;

    private InputDevice targetDevice;

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

        if (Input.GetKeyDown("space") && isListening)
        {
            actionDetected = true;
        }
    }

    public void startListening(float duration)
    {
        actionDetected = false;
        StartCoroutine(Listen(duration));
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
