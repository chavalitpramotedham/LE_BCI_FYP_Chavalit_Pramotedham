using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2D : MonoBehaviour
{
    private bool dataCollectionMode;
    private int trialsPerBlockPerType;
    private int totalNumBlocks;

    private int trialsPerBlock;

    private List<bool> trialSequence;

    public int cur_block = 0;
    public int cur_trial = 0;

    public bool block_running = false;
    public bool block_started = false;
    public bool in_trial = false;

    public GameObject gamePanel2D;
    public float readyTime = 3f;

    public GameObject soundSystem;

    void Awake()
    {
        // Everything set here will not change throughout the game!

        // Get Game Settings
        dataCollectionMode = GetComponent<GameSettings>().dataCollectionMode;

        totalNumBlocks = GetComponent<GameSettings>().blocksIs2DGameMode.Length;
        trialsPerBlockPerType = GetComponent<GameSettings>().trialsPerBlockPerType;

        gamePanel2D.SetActive(false);
        gamePanel2D.GetComponent<GamePanelBehaviour2D>().enabled = false;
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
        gamePanel2D.SetActive(true);
        gamePanel2D.GetComponent<GamePanelBehaviour2D>().enabled = true;

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
            print("ERROR. 2D cannot be used in non-data-collection mode");
            return;
        }

        StartCoroutine("startBlock");
    }

    private IEnumerator startBlock()
    {
        // START Sequence (S)

        DAQ_Manager.setFlag("S2D");

        int i = 0;

        while (i < readyTime)
        {
            gamePanel2D.GetComponent<GamePanelBehaviour2D>().updateCountdown((int)(readyTime - i));
            yield return new WaitForSeconds(1f);
            i += 1;
        }

        gamePanel2D.GetComponent<GamePanelBehaviour2D>().updateCountdown(0);

        block_started = true;
        cur_trial = 0;
        in_trial = false;
    }

    private IEnumerator nextTrial()
    {
        in_trial = true;

        bool nextRoundActive = trialSequence[cur_trial];
        print("SEQ: " + cur_block + " ROUND: " + cur_trial + " TYPE: " + trialSequence[cur_trial]);

        // Random wait for 3-5 seconds

        System.Random r = new System.Random();

        yield return new WaitForSeconds(r.Next(3, 6));

        // Countdown for ready

        soundSystem.GetComponent<SoundSystemManager>().beep_2D();
        gamePanel2D.GetComponent<GamePanelBehaviour2D>().setReady(cur_block, totalNumBlocks, cur_trial, trialsPerBlock);

        yield return new WaitForSeconds(3f);

        // show BCI Indicator

        if (nextRoundActive)
        {
            DAQ_Manager.setFlag("B2D");
        }
        else
        {
            DAQ_Manager.setFlag("R2D");
        }

        soundSystem.GetComponent<SoundSystemManager>().beep_2D();
        gamePanel2D.GetComponent<GamePanelBehaviour2D>().setBCIIndicator(nextRoundActive);

        yield return new WaitForSeconds(5f);

        DAQ_Manager.setFlag("I2D");
        soundSystem.GetComponent<SoundSystemManager>().beep_2D();
        gamePanel2D.GetComponent<GamePanelBehaviour2D>().deactivateAll();

        finishTrial();
    }

    public void finishTrial()
    {
        cur_trial += 1;
        in_trial = false;
    }

    private void finishBlock()
    {
        // FINISH SEQUENCE

        DAQ_Manager.setFlag("E2D");

        block_running = false;
    }

    private void OnDisable()
    {
        gamePanel2D.SetActive(false);
        gamePanel2D.GetComponent<GamePanelBehaviour2D>().enabled = false;

        block_running = false;
        block_started = false;
        in_trial = false;
    }
}
