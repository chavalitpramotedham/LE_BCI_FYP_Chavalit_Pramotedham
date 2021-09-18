using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownPanelBehavior : MonoBehaviour
{
    private List<Color32> colors = new List<Color32>();

    public GameObject CD;
    public GameObject Success;
    public GameObject Failure;

    public Image countdownImage;

    public TextMeshProUGUI countdownText;

    void Awake()
    {
        colors.Add(new Color32(210, 43, 11, 255)); // 1s left (red)
        colors.Add(new Color32(231, 130, 13, 255));
        colors.Add(new Color32(231, 218, 13, 255));
        colors.Add(new Color32(127, 169, 0, 255));
        colors.Add(new Color32(0, 108, 13, 255)); // 5s left (green)
    }

    public void startCountdown(float duration)
    {
        Success.SetActive(false);
        Failure.SetActive(false);

        CD.SetActive(true);

        countdownImage.fillAmount = 1;
        countdownImage.color = colors[colors.Count - 1];

        StartCoroutine(Countdown(duration));
    }


    private IEnumerator Countdown(float duration)
    {
        float normalizedTime = 0.99999f;

        while (normalizedTime >= 0f)
        {
            normalizedTime -= Time.deltaTime / duration;

            countdownText.SetText(((int)(normalizedTime * duration)+1).ToString());

            int color_index = (int)(normalizedTime * (float)colors.Count);

            countdownImage.color = colors[color_index];
            countdownImage.fillAmount = normalizedTime;

            yield return null;
        }

        CD.SetActive(false);
    }

    public void setResult(bool success)
    {
        if (success)
        {
            CD.SetActive(false);
            Success.SetActive(true);
        }
        else
        {
            CD.SetActive(false);
            Failure.SetActive(true);
        }
    }

    public void resetUI()
    {
        Success.SetActive(false);
        Failure.SetActive(false);

        CD.SetActive(false);
    }
}
