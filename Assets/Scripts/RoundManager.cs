using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoundManager : MonoBehaviour
{
    private bool started = false;

    private int stage = 0;
    private bool inStage = false;

    public float readyTime = 3f;
    public float bciTaskTime = 5f;
    public float kickAimTime = 3f;
    public float finishTime = .5f;

    public GameObject targetSystem;
    private TargetManager targetManager;
    private InputManager inputManager;

    public GameObject mainCamera;

    public GameObject gameBall;

    public GameObject readyPanel;
    public GameObject bciPanel;
    public GameObject kickAimPanel;
    public GameObject pointsPanel;
    public GameObject countdownPanel;

    // Start is called before the first frame update
    void Start()
    {
        targetManager = targetSystem.GetComponent<TargetManager>();

        inputManager = gameObject.GetComponent<InputManager>();

        readyPanel.SetActive(false);
        bciPanel.SetActive(false);
        kickAimPanel.SetActive(false);
        pointsPanel.SetActive(false);

        StartCoroutine("ReadyCountdown");
    }

    // Update is called once per frame
    void Update()
    {
        if (started && !inStage) {
            switch (stage)
            {
                case 0:
                    // 1. start Target movement + look @ movement

                    if (!targetManager.isActivatedForRound)
                    {
                        StartCoroutine("startStage0");
                    }
                    break;

                case 1:
                    StartCoroutine("startStage1");
                    break;

                case 2:
                    StartCoroutine("startStage2");
                    break;

                case 3:
                    StartCoroutine("startStage3");
                    break;

                case 4:
                    StartCoroutine("startStage4");
                    break;
            }
        }

        if (Input.GetKeyDown("space") && stage == 5 && !inStage)
        {
            inStage = false;
            stage = 0;
        }
    }

    private IEnumerator startStage0()
    {
        inStage = true;

        targetManager.startTargetRound();

        float duration = 0.5f;
        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        stage = 1;
        inStage = false;
    }

    private IEnumerator startStage1()
    {
        inStage = true;

        bciPanel.SetActive(true);
        bciPanel.GetComponent<BCIPanelBehavior>().showInstruction();
        countdownPanel.GetComponent<CountdownPanelBehavior>().startCountdown(bciTaskTime);
        mainCamera.GetComponent<CameraRoundMovement>().moveForward(bciTaskTime);

        inputManager.startListening(bciTaskTime);

        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / bciTaskTime;

            yield return null;
        }

        stage = 2;
        inStage = false;
    }


    private IEnumerator startStage2()
    {
        inStage = true;

        if (inputManager.getActionDetected())
        {
            mainCamera.GetComponent<CameraRoundMovement>().setPlayerWait();

            //If BCI output == true (or simulated = space bar is tapped)
            // indicate tick on time bar

            bciPanel.GetComponent<BCIPanelBehavior>().setResult(true);
            countdownPanel.GetComponent<CountdownPanelBehavior>().setResult(true);
            targetManager.startTargetMovement();

            yield return new WaitForSeconds(1f);

            // and allow aim

            gameBall.GetComponent<BallLaunch>().enabled = true;
            gameBall.GetComponent<DrawTrajectory>().enabled = true;

            stage = 3;
            inStage = false;
        }
        else
        {
            //else indicate X on time bar
            bciPanel.GetComponent<BCIPanelBehavior>().setResult(false);
            countdownPanel.GetComponent<CountdownPanelBehavior>().setResult(false);

            yield return new WaitForSeconds(1f);

            //and go to stage = 4

            stage = 4;
            inStage = false;
        }
    }

    private IEnumerator startStage3()
    {
        inStage = true;

        bciPanel.SetActive(false);

        kickAimPanel.SetActive(true);
        countdownPanel.GetComponent<CountdownPanelBehavior>().startCountdown(kickAimTime);

        gameBall.GetComponent<DrawTrajectory>().startCountdown(kickAimTime);

        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / kickAimTime;

            yield return null;
        }

        kickAimPanel.SetActive(false);

        //Shoot!
        mainCamera.GetComponent<CameraRoundMovement>().setPlayerKick();
        gameBall.GetComponent<BallLaunch>().setToShoot();
        gameObject.GetComponent<PointsManager>().addKick();

        yield return new WaitForSeconds(2.5f);

        // Disable scripts
        gameBall.GetComponent<BallLaunch>().enabled = false;
        gameBall.GetComponent<DrawTrajectory>().enabled = false;

        // go to stage 4
        stage = 4;
        inStage = false;
    }

    private IEnumerator startStage4()
    {
        inStage = true;

        // 6. Wait 2.5 seconds (as usual) for target&ball to drop

        mainCamera.GetComponent<CameraRoundMovement>().moveBackward(finishTime);

        yield return new WaitForSeconds(finishTime);

        targetManager.isActivatedForRound = false;
        bciPanel.SetActive(false);
        kickAimPanel.SetActive(false);
        countdownPanel.GetComponent<CountdownPanelBehavior>().resetUI();

        stage = 5;
        inStage = false;
    }

    private IEnumerator ReadyCountdown()
    {
        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            readyPanel.SetActive(true);

            normalizedTime += Time.deltaTime / readyTime;

            int roundedTime = 1 + (int)(readyTime - (normalizedTime * readyTime));

            readyPanel.GetComponent<ReadyPanelBehavior>().updateCountdown(roundedTime);

            yield return null;
        }

        readyPanel.SetActive(false);
        pointsPanel.SetActive(true);

        started = true;

    }
}
