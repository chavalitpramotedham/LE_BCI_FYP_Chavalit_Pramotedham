using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockStartPanelBehavior : MonoBehaviour
{
    public TextMeshProUGUI blockIndicatorText;
    public void updateBlockNumUI(int blockIndex)
    {
        blockIndicatorText.SetText("Block: " + (blockIndex + 1).ToString());
    }
}
