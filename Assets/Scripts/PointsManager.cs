using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    private int points = 0;
    private int kicks = 0;

    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI kicksText;
    public Image badge;
    public GameObject soundSystem;

    private void Start()
    {
        resetBadge();
        updatePointsUI();
        kicksText.SetText("Kicks: 0");
    }

    public void addPoints(int addition)
    {
        soundSystem.GetComponent<SoundSystemManager>().cheer();

        points += addition;

        setBadge(addition);
    }

    private void updatePointsUI()
    {
        pointsText.SetText(points.ToString());
    }

    public void addKick()
    {
        kicks += 1;
        kicksText.SetText("Kicks: " + kicks.ToString());
    }

    private IEnumerator Countdown()
    {
        float duration = 1.25f;

        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        resetBadge();
        updatePointsUI();
    }

    private void setBadge(int addition)
    {
        var tempColor = badge.color;
        tempColor.a = 1f;
        badge.color = tempColor;
        badge.sprite = Resources.Load<Sprite>(addition.ToString() + "pts");

        StartCoroutine("Countdown");
    }

    private void resetBadge()
    {
        badge.sprite = null;
        var tempColor = badge.color;
        tempColor.a = 0f;
        badge.color = tempColor;
    }
}
