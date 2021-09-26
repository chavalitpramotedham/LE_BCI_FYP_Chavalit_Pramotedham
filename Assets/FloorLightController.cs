using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLightController : MonoBehaviour
{
    public Material light_nothing;
    public Material light_count;
    public Material light_ball_active;
    public Material light_ball_inactive;

    public GameObject light_1;
    public GameObject light_2;
    public GameObject light_3;

    public void startCountdown(bool active)
    {
        StartCoroutine(Countdown(active));
    }

    private IEnumerator Countdown(bool active)
    {
        float normalizedTime = 0;
        float duration = 3;

        while (normalizedTime < 1f)
        {
            normalizedTime += Time.deltaTime / duration;

            if (normalizedTime > 0)
            {
                Material[] materials = light_1.GetComponent<MeshRenderer>().materials;
                materials[0] = light_count;
                light_1.GetComponent<MeshRenderer>().materials = materials;
            }

            if (normalizedTime > 0.3333333)
            {
                Material[] materials = light_2.GetComponent<MeshRenderer>().materials;
                materials[0] = light_count;
                light_2.GetComponent<MeshRenderer>().materials = materials;
            }

            if (normalizedTime > 0.6666667)
            {
                Material[] materials = light_3.GetComponent<MeshRenderer>().materials;
                materials[0] = light_count;
                light_3.GetComponent<MeshRenderer>().materials = materials;
            }
            yield return null;
        }

        print("TYPE: " + active);

        if (active)
        {
            Material[] materials = light_1.GetComponent<MeshRenderer>().materials;
            materials[0] = light_ball_active;
            light_1.GetComponent<MeshRenderer>().materials = materials;
            light_2.GetComponent<MeshRenderer>().materials = materials;
            light_3.GetComponent<MeshRenderer>().materials = materials;
        }

        else
        {
            Material[] materials = light_1.GetComponent<MeshRenderer>().materials;
            materials[0] = light_ball_inactive;
            light_1.GetComponent<MeshRenderer>().materials = materials;
            light_2.GetComponent<MeshRenderer>().materials = materials;
            light_3.GetComponent<MeshRenderer>().materials = materials;
        }
    }

    public void resetLights()
    {
        Material[] materials = light_1.GetComponent<MeshRenderer>().materials;
        materials[0] = light_nothing;
        light_1.GetComponent<MeshRenderer>().materials = materials;
        light_2.GetComponent<MeshRenderer>().materials = materials;
        light_3.GetComponent<MeshRenderer>().materials = materials;
    }
}
