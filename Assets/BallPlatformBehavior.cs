using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlatformBehavior : MonoBehaviour
{
    public GameObject indicator;
    public GameObject ball;

    public Material indicator_neutral;
    public Material indicator_active;
    public Material indicator_inactive;

    private Vector3 indicatorStartPos;
    private Vector3 indicatorEndPos;

    private void Start()
    {
        indicatorStartPos = transform.position;
        indicatorEndPos = indicatorStartPos + new Vector3(0, 0.032f, 0);
    }

    public void startRaisePlatform(bool active)
    {
        StartCoroutine(raisePlatform(active));
    }

    private IEnumerator raisePlatform(bool active)
    {
        print("RAISING");

        Material[] materials = indicator.GetComponent<MeshRenderer>().materials;
        materials[0] = indicator_neutral;
        indicator.GetComponent<MeshRenderer>().materials = materials;

        // lerp for 3s from y = 0.18 to 0.22
        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / 3;

            transform.position = Vector3.Lerp(indicatorStartPos, indicatorEndPos, normalizedTime);

            yield return null;
        }

        if (active)
        {
            materials = indicator.GetComponent<MeshRenderer>().materials;
            materials[0] = indicator_active;
            indicator.GetComponent<MeshRenderer>().materials = materials;

            claimBall();
        }
        else
        {
            materials = indicator.GetComponent<MeshRenderer>().materials;
            materials[0] = indicator_inactive;
            indicator.GetComponent<MeshRenderer>().materials = materials;
        }
    }

    private void claimBall()
    {
        ball.transform.localPosition = new Vector3(0,1.265f,0);
        ball.SetActive(true);
    }

    public void startLowerPlatform()
    {
        StartCoroutine("lowerPlatform");
    }

    private IEnumerator lowerPlatform()
    {
        ball.SetActive(false);

        Material[] materials = indicator.GetComponent<MeshRenderer>().materials;
        materials[0] = indicator_neutral;
        indicator.GetComponent<MeshRenderer>().materials = materials;

        // lerp for 3s from y = 0.18 to 0.22
        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / .5f;

            transform.position = Vector3.Lerp(indicatorEndPos, indicatorStartPos, normalizedTime);

            yield return null;
        }
    }
}
