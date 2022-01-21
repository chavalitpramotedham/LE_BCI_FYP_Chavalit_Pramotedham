using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest_Movement : MonoBehaviour
{
    private bool isLeftSide;

    public Transform hand;
    Vector3 originalHandPos;
    Vector3 originalHandRot;

    public GameObject restBall;
    public Vector3 originalBallScale;
    float finalBallScaleFloat;
    public Vector3 finalBallScale;

    private AudioClip squeeze_in;
    private AudioClip squeeze_out;

    public GameObject VR_camera;

    float normalizedTime;
    float movement_multiplier;

    public bool toMove;
    public bool moving;
    public int iterations;

    void Start()
    {
        squeeze_in = (AudioClip)Resources.Load("Sounds/squeeze_in");
        squeeze_out = (AudioClip)Resources.Load("Sounds/squeeze_out");

        originalHandPos = hand.position;
        originalHandRot = hand.localEulerAngles;
    }

    public void setLeftSide(bool isLeft)
    {
        isLeftSide = isLeft;

        if (isLeftSide)
        {
            movement_multiplier = -2;
        }
        else
        {
            movement_multiplier = 2;
        }
    }

    public void activate()
    {
        toMove = true;
        moving = false;
        iterations = 0;

        originalBallScale = restBall.transform.localScale;
        finalBallScaleFloat = originalBallScale.x * 0.8f;
        finalBallScale = new Vector3(finalBallScaleFloat, finalBallScaleFloat, finalBallScaleFloat);

        VR_camera.GetComponent<Camera>().nearClipPlane = 0.2f;
    }

    public void deactivate()
    {
        toMove = false;
        moving = false;

        restBall.transform.localScale = originalBallScale;

        VR_camera.GetComponent<Camera>().nearClipPlane = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (toMove && !moving)
        {
            moving = true;
            hand.localEulerAngles = originalHandRot;
            StartCoroutine("moveArm");
        }
    }

    private IEnumerator moveArm()
    {
        float duration = .5f;

        normalizedTime = 0; 
        bool squeaked = false;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            hand.RotateAround(originalHandPos, hand.forward, -normalizedTime * movement_multiplier);

            restBall.transform.localScale = Vector3.Lerp(originalBallScale, finalBallScale, normalizedTime);

            if (normalizedTime > 0.75f && !squeaked)
            {
                squeaked = true;
                // restBall.GetComponent<AudioSource>().PlayOneShot(squeeze_in);
            }

            yield return null;
        }

        normalizedTime = 1;

        squeaked = false;

        while (normalizedTime >= 0f)
        {
            normalizedTime -= Time.deltaTime / duration;
            hand.RotateAround(originalHandPos, hand.forward, normalizedTime * movement_multiplier);

            restBall.transform.localScale = Vector3.Lerp(originalBallScale, finalBallScale, normalizedTime);

            if (normalizedTime < 0.5f && !squeaked)
            {
                squeaked = true;
                // restBall.GetComponent<AudioSource>().PlayOneShot(squeeze_out);
            }

            yield return null;
        }

        hand.localEulerAngles = originalHandRot;

        moving = false;
        iterations += 1;

        yield return null;
    }
}
