using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    private bool dataCollectionMode;
    private bool isLeftSide;
    public GameObject Avatars;
    private int trialsPerBlockPerType;
    private int totalNumBlocks;

    private int trialsPerBlock;

    private List<bool> trialSequence;

    public int cur_block = 0;
    public int cur_trial = 0;

    public bool block_running = false;
    public bool block_started = false;
    public bool in_trial = false;

    private RoundManager roundManager;
    private RestManager restManager;
    private InputManager inputManager;
    private PointsManager pointsManager;

    public GameObject startPanel;

    public GameObject readyPanel;
    public float readyTime = 3f;

    public GameObject roundPanel;
    
    public GameObject pointsPanel;

    public GameObject endPanel;

    public GameObject ballSpawner;

    public GameObject soundSystem;

    // Start is called before the first frame update
    void Awake()
    {
        // Everything set here will not change throughout the game!

        // Get Game Settings
        dataCollectionMode = GetComponent<GameSettings>().dataCollectionMode;
        isLeftSide = GetComponent<GameSettings>().isLeftSide;

        totalNumBlocks = GetComponent<GameSettings>().numBlocksAndIs2DGameMode.Length;
        trialsPerBlockPerType = GetComponent<GameSettings>().trialsPerBlockPerType;

        // Set Collection Mechanism

        GetComponent<InputManager>().dataCollectionMode = dataCollectionMode;

        // Process Avatar Direction
        Avatars.GetComponent<Avatar_Controller>().setLeftSide(isLeftSide);

        // Find objects
        roundManager = GetComponent<RoundManager>();
        restManager = GetComponent<RestManager>();
        pointsManager = GetComponent<PointsManager>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Automated round advancing

        if (block_running && block_started && !in_trial)
        {
            if (cur_trial < trialsPerBlock)
            {
                StartCoroutine("nextTrial");
            }
            else
            {
                finishBlock();
            }
        }
    }

    private void OnEnable()
    {
        // Wake Scripts
        roundManager.enabled = true;
        restManager.enabled = true;
        pointsManager.enabled = true;
        inputManager.enabled = true;

        startPanel.SetActive(true);
        pointsPanel.SetActive(true);
        readyPanel.SetActive(false);
        roundPanel.SetActive(false);
        endPanel.SetActive(false);

        // Create game sequences

        if (dataCollectionMode)
        {
            trialsPerBlock = 2 * trialsPerBlockPerType;

            trialSequence = new List<bool>();
            int round_count = 0;
            int rest_count = 0;

            System.Random rand = new System.Random();

            for (int j = 0; j < trialsPerBlock; j++)
            {
                if (round_count < trialsPerBlockPerType && rest_count < trialsPerBlockPerType)
                {
                    if (round_count <= rest_count - 2)
                    {
                        trialSequence.Add(true);
                        round_count += 1;
                    }
                    else if (rest_count <= round_count - 2)
                    {
                        trialSequence.Add(false);
                        rest_count += 1;
                    }
                    else
                    {
                        int value = rand.Next(0, 2);

                        if (value == 0)
                        {
                            trialSequence.Add(false);
                            rest_count += 1;
                        }

                        else
                        {
                            trialSequence.Add(true);
                            round_count += 1;
                        }
                    }
                }

                else if (rest_count < trialsPerBlockPerType && round_count == trialsPerBlockPerType)
                {
                    while (rest_count < trialsPerBlockPerType)
                    {
                        trialSequence.Add(false);
                        rest_count += 1;
                    }
                }

                else if (round_count < trialsPerBlockPerType && rest_count == trialsPerBlockPerType)
                {
                    while (round_count < trialsPerBlockPerType)
                    {
                        trialSequence.Add(true);
                        round_count += 1;
                    }
                }
            }
        }
        else
        {
            trialsPerBlock = trialsPerBlockPerType;

            trialSequence = new List<bool>();

            for (int j = 0; j < trialsPerBlock; j++)
            {
                trialSequence.Add(true);
            }
        }

        StartCoroutine("startBlock");
    }

    private IEnumerator startBlock()
    {
        // START Sequence (S)

        DAQ_Manager.setFlag("S");

        startPanel.SetActive(false);

        soundSystem.GetComponent<SoundSystemManager>().start_sequence();

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

        block_started = true;
        cur_trial = 0;
        in_trial = false;
    }

    private IEnumerator nextTrial()
    {
        in_trial = true;
        roundPanel.SetActive(true);
        roundPanel.GetComponent<RoundPanelBehavior>().updateUI(cur_block, totalNumBlocks, cur_trial, trialsPerBlock);

        bool nextRoundActive = trialSequence[cur_trial];

        print("SEQ: "+cur_block+ " ROUND: "+cur_trial + " TYPE: "+ trialSequence[cur_trial]);

        System.Random r = new System.Random();

        yield return new WaitForSeconds(r.Next(3, 6));

        ballSpawner.GetComponent<BallSpawner>().startRound(nextRoundActive);

        for (int i = 0; i < 3; i++)
        {
            soundSystem.GetComponent<SoundSystemManager>().count();
            yield return new WaitForSeconds(1f);
        }

        roundPanel.SetActive(false);

        if (nextRoundActive)
        {
            soundSystem.GetComponent<SoundSystemManager>().round();
            roundManager.startRound();
        }
        else
        {
            soundSystem.GetComponent<SoundSystemManager>().rest();
            restManager.startRest();
        }
    }

    public void finishTrial()
    {
        cur_trial += 1;
        in_trial = false;

        ballSpawner.GetComponent<BallSpawner>().resetRound();
    }

    private void finishBlock()
    {
        StartCoroutine("wait1sec");

        // FINISH SEQUENCE

        DAQ_Manager.setFlag("E");

        block_running = false;

        soundSystem.GetComponent<SoundSystemManager>().finish_sequence();

        pointsPanel.SetActive(true);
        readyPanel.SetActive(false);
        roundPanel.SetActive(false);
    }

    private void OnDisable()
    {
        roundManager.enabled = false;
        restManager.enabled = false;
        pointsManager.enabled = false;
        inputManager.enabled = false;

        startPanel.SetActive(false);
        pointsPanel.SetActive(false);
        readyPanel.SetActive(false);
        roundPanel.SetActive(false);
        endPanel.SetActive(false);

        block_running = false;
        block_started = false;
        in_trial = false;
    }

    private IEnumerator wait1sec()
    {
        yield return new WaitForSeconds(1f);
    }
}
