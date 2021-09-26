using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    [Range(1, 4)]
    public int sequencesPerGame = 1;

    [SerializeField]
    [Range(1, 25)]
    public int roundsPerSequencePerType = 10;

    private int roundsPerSequence;

    private List<List<bool>> sequences = new List<List<bool>>();

    public int cur_sequence = 0;
    public int cur_round = 0;

    public bool in_sequence = false;
    public bool in_round = false;

    private RoundManager roundManager;
    private RestManager restManager;

    public GameObject startPanel;
    public GameObject readyPanel;
    public float readyTime = 3f;
    public GameObject pointsPanel;

    public GameObject ballSpawner;

    private InputDevice targetDevice;

    // Start is called before the first frame update
    void Awake()
    {
        // Attach target device

        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }

        // Find objects
        roundManager = GetComponent<RoundManager>();
        restManager = GetComponent<RestManager>();

        startPanel.SetActive(true);
        pointsPanel.SetActive(true);
        readyPanel.SetActive(false);

        // Create game sequences

        roundsPerSequence = 2 * roundsPerSequencePerType;

        for (int i = 0; i < sequencesPerGame; i++)
        {
            List<bool> sequence = new List<bool>();
            int round_count = 0;
            int rest_count = 0;

            System.Random rand = new System.Random();

            for (int j = 0; j < roundsPerSequence; j++)
            {
                if (round_count < roundsPerSequencePerType && rest_count < roundsPerSequencePerType)
                {
                    if (round_count <= rest_count - 2)
                    {
                        sequence.Add(true);
                        round_count += 1;
                    }
                    else if (rest_count <= round_count - 2)
                    {
                        sequence.Add(false);
                        rest_count += 1;
                    }
                    else
                    {
                        int value = rand.Next(0, 2);

                        if (value == 0)
                        {
                            sequence.Add(false);
                            rest_count += 1;
                        }

                        else
                        {
                            sequence.Add(true);
                            round_count += 1;
                        }
                    }
                }

                else if (rest_count < roundsPerSequencePerType && round_count == roundsPerSequencePerType)
                {
                    while (rest_count < roundsPerSequencePerType)
                    {
                        sequence.Add(false);
                        rest_count += 1;
                    }
                }

                else if (round_count < roundsPerSequencePerType && rest_count == roundsPerSequencePerType)
                {
                    while (round_count < roundsPerSequencePerType)
                    {
                        sequence.Add(true);
                        round_count += 1;
                    }
                }
            }

            sequences.Add(sequence);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Sequence start handling (by I/O)

        if (Input.GetKeyDown("space") && !in_sequence && cur_sequence < sequencesPerGame)
        {
            StartCoroutine("startSequence");
        }

        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);

        if (triggerValue > 0.1f && !in_sequence && cur_sequence < sequencesPerGame)
        {
            StartCoroutine("startSequence");
        }

        // Automated round advancing

        if (in_sequence && !in_round)
        {
            if (cur_round < roundsPerSequence)
            {
                StartCoroutine("nextRound");
            }
            else
            {
                finishSequence();
            }
        }

        // Finish Game

        if (cur_sequence == sequencesPerGame)
        {
            print("END");
        }
    }

    private IEnumerator startSequence()
    {
        startPanel.SetActive(false);

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

        in_sequence = true;
        cur_round = 0;
        in_round = false;
    }

    private IEnumerator nextRound()
    {
        in_round = true;
        bool nextRoundActive = sequences[cur_sequence][cur_round];

        print("SEQ: "+cur_sequence+ " ROUND: "+cur_round + " TYPE: "+sequences[cur_sequence][cur_round]);

        yield return new WaitForSeconds(1f);

        ballSpawner.GetComponent<BallSpawner>().startRound(nextRoundActive);

        yield return new WaitForSeconds(3f);

        if (nextRoundActive)
        {
            roundManager.startRound();
        }
        else
        {
            restManager.startRest();
        }
    }

    public void finishRound()
    {
        print("Finish Round");

        cur_round += 1;
        in_round = false;

        ballSpawner.GetComponent<BallSpawner>().resetRound();
    }

    private void finishSequence()
    {
        cur_sequence += 1;
        in_sequence = false;

        startPanel.SetActive(true);
        pointsPanel.SetActive(true);
        readyPanel.SetActive(false);
    }

    
}
