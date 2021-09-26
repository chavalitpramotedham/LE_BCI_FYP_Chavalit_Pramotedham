using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject floor_light_1;
    public GameObject floor_light_2;

    public GameObject ballPlatform;

    // Start is called before the first frame update
    void Start()
    {
        resetFloorLights();

    }

    public void startRound(bool active)
    {
        initiateFloorLights(active);
        ballPlatform.GetComponent<BallPlatformBehavior>().startRaisePlatform(active);
    }

    private void initiateFloorLights(bool active)
    {
        floor_light_1.GetComponent<FloorLightController>().startCountdown(active);
        floor_light_2.GetComponent<FloorLightController>().startCountdown(active);
    }

    public void resetRound()
    {
        resetFloorLights();
        ballPlatform.GetComponent<BallPlatformBehavior>().startLowerPlatform();
    }

    private void resetFloorLights()
    {
        floor_light_1.GetComponent<FloorLightController>().resetLights();
        floor_light_2.GetComponent<FloorLightController>().resetLights();
    }
}
