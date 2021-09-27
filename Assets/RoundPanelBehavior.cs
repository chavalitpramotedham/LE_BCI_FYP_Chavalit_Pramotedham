using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundPanelBehavior : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    public void updateUI(int roundNum, int totalRounds)
    {
        string display = "Round: " + (roundNum+1).ToString() + "/" + totalRounds.ToString();
        displayText.SetText(display);
    }
}
