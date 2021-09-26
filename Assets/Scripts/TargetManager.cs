using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private GameObject gameManager;
    private int targetMovementPoints;
    private int targetMovementSpeed;

    public bool isActivatedForRound;

    public GameObject targetSurface;
    public GameObject target;

    private float x_min = -6.0f;
    private float x_max = 2.3f;
    private float y_min = 0.6f;
    private float y_max = 2.6f;

    private float z_fixed = -34.71f;

    private int difficulty = 1;

    private List<Vector3> targetMovementVectors;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        targetMovementPoints = gameManager.GetComponent<GameSettings>().targetMovementPoints;
        targetMovementSpeed = gameManager.GetComponent<GameSettings>().targetMovementSpeed;
        isActivatedForRound = false;
    }
    // Update is called once per frame
    void Update()
    {
        targetMovementPoints = gameManager.GetComponent<GameSettings>().targetMovementPoints;
        targetMovementSpeed = gameManager.GetComponent<GameSettings>().targetMovementSpeed;

        if (target.GetComponent<TargetBehavior>().getIsHit() == true)
        {
            isActivatedForRound = false;
        }
    }

    public void startTargetRound()
    {
        if (target.GetComponent<TargetBehavior>().getIsHit() == null)
        {
            target.SetActive(true);
            isActivatedForRound = true;
            arrangeTarget();

        }
        else if (target.GetComponent<TargetBehavior>().getIsHit() == true)
        {
            target.SetActive(true);
            isActivatedForRound = true;
            arrangeTarget();
        }
    }

    private void arrangeTarget()
    {
        setTargetMovementPoints(targetMovementPoints);

        setTargetPositions();
    }

    private void setTargetMovementPoints(int numPoints)
    {
        targetMovementVectors = new List<Vector3>();

        for (int i = 0; i < numPoints; i++)
        {
            targetMovementVectors.Add(randomizeTargetPosition());
        }
    }

    private void setTargetPositions()
        {
            target.GetComponent<TargetBehavior>().reset();
            target.gameObject.GetComponent<TargetMovement>().moveBetweenPoints(targetMovementVectors, targetMovementSpeed);
        }

    private Vector3 randomizeTargetPosition()
    {
        Vector3 pos = new Vector3();

        pos.x = randomX();
        pos.y = randomY();
        pos.z = z_fixed;

        return pos;
    }
    
    private float randomX()
    {
        return Random.Range(x_min, x_max);
    }

    private float randomY()
    {
        return Random.Range(y_min, y_max);
    }

    public void startTargetMovement()
    {
        target.gameObject.GetComponent<TargetMovement>().enableMovement();
    }

    public void stopTargetMovement()
    {
        target.gameObject.GetComponent<TargetMovement>().disableMovement();
    }
}
