using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    public bool dataCollectionMode = true;

    [SerializeField]
    public bool engagementMode = true;

    [SerializeField]
    public bool isLeftSide = false;

    [SerializeField]
    public bool[] numBlocksAndIs2DGameMode;

    [SerializeField]
    [Range(1, 20)]
    public int trialsPerBlockPerType = 10;

    [SerializeField]
    [Range(1, 10)]
    public int targetMovementPoints = 1;

    [SerializeField]
    [Range(1, 10)]
    public int targetMovementSpeed = 1;

    public int curBlockIndex = 0;
    public bool inBlock = false;

    private GameManager gmVR;
    private GameManager2D gm2D;

    public GameObject blockStartPanel;
    public GameObject blockStartPanelVR;
    public GameObject gameEndPanel;

    void Awake()
    {
        gmVR = gameObject.GetComponent<GameManager>();
        gm2D = gameObject.GetComponent<GameManager2D>();

        if (!dataCollectionMode)
        {
            for (int i = 0; i < numBlocksAndIs2DGameMode.Length; i++)
            {
                if (numBlocksAndIs2DGameMode[i])
                {
                    print("Invalid Settings. 2D game mode is only for data collection. Please Restart");
                    this.enabled = false;
                }
            }
        }

        activateStartScreen();
    }

    private void Update()
    {
        if (!inBlock)
        {
            if (curBlockIndex < numBlocksAndIs2DGameMode.Length && Input.GetKeyDown("space"))
            {
                inBlock = true;
                nextBlock();
            }
        }
        else
        {
            if (numBlocksAndIs2DGameMode[curBlockIndex])
            {
                if (!gm2D.block_running && gm2D.block_started)
                {
                    gm2D.enabled = false;
                    endBlock();
                }
            }
            else
            {
                if (!gmVR.block_running && gmVR.block_started)
                {
                    gmVR.enabled = false;
                    endBlock();
                }
            }
        }
    }

    private void nextBlock()
    {
        if (numBlocksAndIs2DGameMode[curBlockIndex])
        {
            gm2D.cur_block = curBlockIndex;
            gm2D.block_running = true;
            gm2D.enabled = true;
        }
        else
        {
            gmVR.cur_block = curBlockIndex;
            gmVR.block_running = true;
            gmVR.enabled = true;
        }

        deactiveStartScreen();
    }

    private void endBlock()
    {
        curBlockIndex += 1;
        inBlock = false;

        if (curBlockIndex == numBlocksAndIs2DGameMode.Length)
        {
            activateEndScreen();
        }
        else
        {
            activateStartScreen();
        }
    }

    private void activateStartScreen()
    {
        blockStartPanel.SetActive(true);
        blockStartPanelVR.SetActive(true);
        gameEndPanel.SetActive(false);
        blockStartPanel.GetComponent<BlockStartPanelBehavior>().updateBlockNumUI(curBlockIndex);
    }

    private void deactiveStartScreen()
    {
        blockStartPanel.SetActive(false);
        blockStartPanelVR.SetActive(false);
        blockStartPanel.GetComponent<BlockStartPanelBehavior>().updateBlockNumUI(curBlockIndex);
    }

    private void activateEndScreen()
    {
        gameEndPanel.SetActive(true);
        blockStartPanel.SetActive(false);
        blockStartPanelVR.SetActive(false);
        print("END");
    }
}
