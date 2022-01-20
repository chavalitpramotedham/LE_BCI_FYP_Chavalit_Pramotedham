using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GamePanelBehaviour2D : MonoBehaviour
{
    public GameObject ready;
    private float readyTime = 3f;

    public GameObject cross;
    public GameObject roundIndicatorText;

    public GameObject upArrow;
    public GameObject downArrow;
    private System.Random random = new System.Random();

    // Start is called before the first frame update

    private void OnEnable()
    {
        ready.SetActive(false);
        ready.GetComponent<TextMeshProUGUI>().SetText("");
        cross.SetActive(false);
        roundIndicatorText.SetActive(false);
        upArrow.SetActive(false);
        downArrow.SetActive(false);
    }

    public void updateCountdown(int num)
    {
        if (num > 0)
        {
            ready.SetActive(true);
            ready.GetComponent<TextMeshProUGUI>().SetText((num).ToString());
        }
        else
        {
            ready.SetActive(false);
            ready.GetComponent<TextMeshProUGUI>().SetText("");
        }
    }

    public void setReady(int curBlock, int totalBlocks, int curTrial, int totalTrials)
    {
        ready.SetActive(false);
        cross.SetActive(true);
        roundIndicatorText.SetActive(true);

        string display = "Block " + (curBlock + 1).ToString() + "/" + totalBlocks.ToString() + " – Task " + (curTrial + 1).ToString() + "/" + totalTrials.ToString();
        roundIndicatorText.GetComponent<TextMeshProUGUI>().SetText(display);
    }

    public void setBCIIndicator(bool isAnkleBCI)
    {
        cross.SetActive(false);
        roundIndicatorText.SetActive(false);

        if (isAnkleBCI)
        {
            downArrow.SetActive(true);
        }
        else
        {
            upArrow.SetActive(true);
        }
    }

    public void deactivateAll()
    {
        downArrow.SetActive(false);
        upArrow.SetActive(false);
        cross.SetActive(false);
        roundIndicatorText.SetActive(false);
    }
}
