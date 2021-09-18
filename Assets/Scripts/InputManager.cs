using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool actionDetected = false;
    private bool isListening = false;

    // Update is called once per frame
    void Update()
    {
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
