using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class RoundManager : MonoBehaviour
{
    private bool started = false;

    public bool engagementMode;

    private int stage = 0;
    private bool inStage = false;

    private int repetitions = 0;

    private float bciTaskTime = 5f;
    private float kickAimTime = 2f;
    private float finishTime = 1.5f;

    public int maxBCIRepetitions = 2;

    public GameObject targetSystem;
    private TargetManager targetManager;
    private InputManager inputManager;

    //public GameObject mainCamera;
    public GameObject player;

    public GameObject gameBall;

    public GameObject bciPanel;
    public GameObject kickAimPanel;
    public GameObject countdownPanel;

    public GameObject floodLights;
    public GameObject soundSystem;

    public GameObject roundAvatar;

    // Start is called before the first frame update
    void Start()
    {
        targetManager = targetSystem.GetComponent<TargetManager>();

        inputManager = gameObject.GetComponent<InputManager>();

        started = false;

        inStage = false;
        stage = 0;

        roundAvatar.SetActive(false);

        bciPanel.SetActive(false);
        kickAimPanel.SetActive(false);

        floodLights.GetComponent<FloodLightController>().setFloodLightsInactive();
    }

    public void startRound()
    {
        started = true;
        stage = 0;
        inStage = false;
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
                    if (!inputManager.isListening){
                        StartCoroutine("startStage2");
                    }
                    break;

                case 3:
                    StartCoroutine("startStage3");
                    break;

                case 4:
                    StartCoroutine("startStage4");
                    break;
            }
        }
    }

    private IEnumerator startStage0()
    {
        // START TARGET ROUND & BUFFER (I)

        inStage = true;

        if (engagementMode){
            targetManager.startTargetRound();
        }

        float duration = 0.5f;
        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        repetitions = 0;

        stage = 1;
        inStage = false;
    }

    private IEnumerator startStage1()
    {
        // BCI STAGE (B)

        DAQ_Manager.setFlag("B");

        inStage = true;

        floodLights.GetComponent<FloodLightController>().setFloodLightsInactive();

        bciPanel.SetActive(true);
        countdownPanel.GetComponent<CountdownPanelBehavior>().startCountdown(stage, bciTaskTime);

        roundAvatar.SetActive(true);
        roundAvatar.GetComponent<Round_Movement>().activate();

        inputManager.startListening(bciTaskTime);

        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / bciTaskTime;

            yield return null;
        }

        roundAvatar.GetComponent<Round_Movement>().deactivate();
        roundAvatar.SetActive(false);

        DAQ_Manager.setFlag("I");

        if (engagementMode){
            countdownPanel.GetComponent<CountdownPanelBehavior>().awaitResult();
            stage = 2;
            inStage = false;
        }
        else{
            endStages();
        }
    }


    private IEnumerator startStage2()
    {
        // Process action (I)

        DAQ_Manager.setFlag("I");

        inStage = true;

        print("STARTING S2");
        yield return new WaitForSeconds(2f);

        if (inputManager.getActionDetected() || repetitions == maxBCIRepetitions)
        {
            floodLights.GetComponent<FloodLightController>().setFloodLightsGreen();
            soundSystem.GetComponent<SoundSystemManager>().success();

            //If BCI output == true (or simulated = space bar is tapped)
            // indicate tick on time bar

            countdownPanel.GetComponent<CountdownPanelBehavior>().setResult(true);
            targetManager.startTargetMovement();

            yield return new WaitForSeconds(1f);

            // and allow aim

            gameBall.GetComponent<BallLaunch>().atShootingStage = true;
            gameBall.GetComponent<DrawTrajectory>().enabled = true;

            stage = 3;
            inStage = false;
        }
        else
        {
            //else indicate X on time bar
            countdownPanel.GetComponent<CountdownPanelBehavior>().setResult(false);

            soundSystem.GetComponent<SoundSystemManager>().failure();
            floodLights.GetComponent<FloodLightController>().setFloodLightsRed();

            yield return new WaitForSeconds(1f);

            //and go to stage = 4

            if (repetitions < maxBCIRepetitions)
            {
                stage = 1;
                inStage = false;
                repetitions += 1;
            }
        }
    }

    private IEnumerator startStage3()
    {
        // KICK STAGE (I)

        DAQ_Manager.setFlag("I");

        inStage = true;

        bciPanel.SetActive(false);

        kickAimPanel.SetActive(true);
        countdownPanel.GetComponent<CountdownPanelBehavior>().startCountdown(stage, kickAimTime);

        gameBall.GetComponent<DrawTrajectory>().startCountdown(kickAimTime);

        float normalizedTime = 0;

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / kickAimTime;

            yield return null;
        }

        kickAimPanel.SetActive(false);

        // Stop all movements, keep UI of target + path, and change title to shoot ball
        targetManager.stopTargetMovement();
        countdownPanel.GetComponent<CountdownPanelBehavior>().shootBall();

        //Shoot!        
        player.GetComponent<playerAnimationController>().setKick();
        gameBall.GetComponent<BallLaunch>().setToShoot();

        yield return new WaitForSeconds(2f);
        
        //mainCamera.GetComponent<CameraRoundMovement>().setPlayerKick();
        gameObject.GetComponent<PointsManager>().addKick();

        yield return new WaitForSeconds(1f);

        // Disable scripts
        gameBall.GetComponent<BallLaunch>().atShootingStage = false;
        gameBall.GetComponent<DrawTrajectory>().enabled = false;

        // go to stage 4
        stage = 4;
        inStage = false;
    }

    private IEnumerator startStage4()
    {
        DAQ_Manager.setFlag("I");

        inStage = true;

        // 6. Wait 2.5 seconds (as usual) for target&ball to drop

        player.GetComponent<playerAnimationController>().setFinish(finishTime);

        yield return new WaitForSeconds(finishTime);

        endStages();

        // started = false;
        // inStage = false;
        // stage = 0;

        // bciPanel.SetActive(false);
        // kickAimPanel.SetActive(false);

        // countdownPanel.GetComponent<CountdownPanelBehavior>().resetUI();
        // floodLights.GetComponent<FloodLightController>().setFloodLightsInactive();

        // GetComponent<GameManager>().finishTrial();
    }

    private void endStages(){
        DAQ_Manager.setFlag("I");

        targetManager.isActivatedForRound = false;

        started = false;
        inStage = false;
        stage = 0;

        bciPanel.SetActive(false);
        kickAimPanel.SetActive(false);

        countdownPanel.GetComponent<CountdownPanelBehavior>().resetUI();
        floodLights.GetComponent<FloodLightController>().setFloodLightsInactive();

        GetComponent<GameManager>().finishTrial();
    }
}
