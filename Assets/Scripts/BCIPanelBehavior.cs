using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BCIPanelBehavior : MonoBehaviour
{
    public GameObject Instructions;
    public GameObject Success;
    public GameObject Failure;

    public void showInstruction()
    {
        Success.SetActive(false);
        Failure.SetActive(false);

        Instructions.SetActive(true);
    }

    public void setResult(bool success)
    {
        if (success)
        {
            Instructions.SetActive(false);
            Success.SetActive(true);
        }
        else
        {
            Instructions.SetActive(false);
            Failure.SetActive(true);
        }
    }
}
