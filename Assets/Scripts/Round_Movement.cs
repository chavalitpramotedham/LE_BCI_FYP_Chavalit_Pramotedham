using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round_Movement : MonoBehaviour
{
    private bool isLeftSide;

    public Transform foot;
    Vector3 originalFootPos;

    public GameObject VR_camera;

    float normalizedTime;
    float movement_multiplier;

    public bool toMove;
    public bool moving;
    public int iterations;

    public void setLeftSide(bool isLeft)
    {
        isLeftSide = isLeft;

        if (isLeftSide)
        {
            movement_multiplier = -1;
        }
        else
        {
            movement_multiplier = 1;
        }
    }

    public void activate()
    {
        toMove = true;
        moving = false;
        iterations = 0;
        originalFootPos = foot.position;

        VR_camera.GetComponent<Camera>().nearClipPlane = 0.2f;
    }

    public void deactivate()
    {
        toMove = false;
        moving = false;
        foot.position = originalFootPos;

        VR_camera.GetComponent<Camera>().nearClipPlane = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (toMove && !moving)
        {
            moving = true;
            StartCoroutine("moveFoot");
        }
    }

    private IEnumerator moveFoot()
    {
        Vector3 originalFootRotation = foot.transform.localEulerAngles;

        float startZ = 0;
        float targetZ = 45;
        float curZ = startZ;

        float duration = .5f;

        normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;

            curZ = -1 * Mathf.Lerp(startZ, targetZ, normalizedTime);

            foot.localEulerAngles = new Vector3(originalFootRotation.x, originalFootRotation.y, curZ);

            //foot.RotateAround(originalFootPos, foot.forward, -normalizedTime * movement_multiplier);

            //print("Going UP");

            yield return null;
        }

        while (normalizedTime >= 0f)
        {
            normalizedTime -= Time.deltaTime / duration;

            curZ = -1 * Mathf.Lerp(startZ, targetZ, normalizedTime);

            foot.localEulerAngles = new Vector3(originalFootRotation.x, originalFootRotation.y, curZ);

            //foot.RotateAround(originalFootPos, foot.forward, normalizedTime * movement_multiplier);

            yield return null;
        }

        moving = false;
        iterations += 1;

        yield return null;

    }
}
