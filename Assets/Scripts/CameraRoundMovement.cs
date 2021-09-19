using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoundMovement : MonoBehaviour
{
    private Vector3 camStartPos;
    private Vector3 camEndPos;

    public bool inKickingPos;

    // Start is called before the first frame update
    void Awake()
    {
        camStartPos = new Vector3(0, 1f, 0);
        camEndPos = new Vector3(0, 1f, 8);

        transform.position = camStartPos;

        inKickingPos = false;
    }

    public void moveForward(float time)
    {
        StartCoroutine(startMoveForward(time));
    }

    private IEnumerator startMoveForward(float moveForwardTime)
    {
        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / moveForwardTime;

            transform.position = Vector3.Lerp(camStartPos, camEndPos, normalizedTime);

            yield return null;
        }

        inKickingPos = true;
    }

    public void moveBackward(float time)
    {
        StartCoroutine(startMoveBackward(time));
    }

    private IEnumerator startMoveBackward(float moveBackwardTime)
    {
        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / moveBackwardTime;

            transform.position = Vector3.Lerp(camEndPos, camStartPos, normalizedTime);

            yield return null;
        }

        inKickingPos = false;
    }

}
