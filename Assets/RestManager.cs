using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestManager : MonoBehaviour
{
    private bool started = false;
    private bool inRest = false;
    private int stage = -1;

    public GameObject restPanel;
    public float restTime = 5f;

    public GameObject countdownPanel;

    public GameObject restAvatar;

    // Start is called before the first frame update
    void Start()
    {
        started = false;

        inRest = false;

        restPanel.SetActive(false);
        restAvatar.SetActive(false);
    }

    public void startRest()
    {
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (started && !inRest)
        {
            StartCoroutine("rest");
        }
    }

    private IEnumerator rest()
    {
        // REST SEQUENCE (R)

        DAQ_Manager.setFlag("R");

        inRest = true;

        restPanel.SetActive(true);
        countdownPanel.GetComponent<CountdownPanelBehavior>().startCountdown(stage, restTime);
        restAvatar.SetActive(true);
        restAvatar.GetComponent<Rest_Movement>().activate();

        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / restTime;
            yield return null;
        }

        endRest();
    }

    private void endRest()
    {
        // END REST (I)

        DAQ_Manager.setFlag("I");

        started = false;
        inRest = false;

        restPanel.SetActive(false);
        countdownPanel.GetComponent<CountdownPanelBehavior>().resetUI();
        restAvatar.SetActive(false);

        GetComponent<GameManager>().finishRound();
    }


}
