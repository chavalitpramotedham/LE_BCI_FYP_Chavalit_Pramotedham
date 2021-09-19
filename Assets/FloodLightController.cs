using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodLightController : MonoBehaviour
{
    public GameObject leftSide;
    public GameObject rightSide;

    public void setFloodLightsInactive()
    {
        leftSide.GetComponent<FloodLightSingleSideController>().setInactive();
        rightSide.GetComponent<FloodLightSingleSideController>().setInactive();
    }

    public void setFloodLightsGreen()
    {
        leftSide.GetComponent<FloodLightSingleSideController>().setGreen();
        rightSide.GetComponent<FloodLightSingleSideController>().setGreen();
    }

    public void setFloodLightsRed()
    {
        leftSide.GetComponent<FloodLightSingleSideController>().setRed();
        rightSide.GetComponent<FloodLightSingleSideController>().setRed();
    }
}
