using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    [Range(1, 10)]
    public int targetMovementPoints = 1;

    [SerializeField]
    [Range(1, 10)]
    public int targetMovementSpeed = 1;

    [SerializeField]
    public bool isBCI = false;
}
