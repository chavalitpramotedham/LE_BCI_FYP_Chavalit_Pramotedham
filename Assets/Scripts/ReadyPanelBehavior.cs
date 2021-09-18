using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadyPanelBehavior : MonoBehaviour
{
    public TextMeshProUGUI countdownText;

    public void updateCountdown(int seconds)
    {
        countdownText.SetText(seconds.ToString());
    }
}
