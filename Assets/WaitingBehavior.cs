using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitingBehavior : MonoBehaviour
{
    public Image waitingImage;
    public TextMeshProUGUI waitingText;
    private bool isAnimating = false;

    // Update is called once per frame
    void Update()
    {
        if (!isAnimating){
            isAnimating = true;
            StartCoroutine("Waiting");
        }
    }

    private IEnumerator Waiting(){
        float duration = 15f;
        float time = 0f;
        
        // Vector3 rotationEuler = new Vector3(0, 0, 0);

        while (time <= duration)
        {
            int numDots = (int)time % 3;

            string displayText = "Processing.";

            for (int i = 0; i < numDots; i++)
            {
                displayText += ".";
            }

            waitingText.GetComponent<TextMeshProUGUI>().SetText(displayText);

            // rotationEuler += Vector3.back * 120 * Time.deltaTime;
            // waitingImage.transform.rotation = Quaternion.Euler(rotationEuler);

            time += Time.deltaTime;
            yield return null;
        }

        isAnimating = false;
    }
}
