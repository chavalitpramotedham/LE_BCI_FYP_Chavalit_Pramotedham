using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    public bool dataCollectionMode = true;

    [SerializeField]
    public bool isLeftSide = false;

    [SerializeField]
    [Range(1, 4)]
    public int sequencesPerGame = 1;

    [SerializeField]
    [Range(1, 25)]
    public int roundsPerSequencePerType = 10;

    [SerializeField]
    [Range(1, 10)]
    public int targetMovementPoints = 1;

    [SerializeField]
    [Range(1, 10)]
    public int targetMovementSpeed = 1;
}
