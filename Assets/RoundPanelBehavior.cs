using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundPanelBehavior : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    public void updateUI(int seqNum, int totalSeq, int roundNum, int totalRounds)
    {
        string display = "Block " + (seqNum + 1).ToString() + "/" + totalSeq.ToString() + " – Task " + (roundNum+1).ToString() + "/" + totalRounds.ToString();
        displayText.SetText(display);
    }
}
