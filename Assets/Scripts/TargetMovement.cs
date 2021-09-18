using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    private TargetBehavior tb;
    private List<Vector3> movementPoints;

    private int cur_index;
    private float step = 0.01f;
    private float speed;

    public bool move = false;

    private void Awake()
    {
        tb = gameObject.GetComponent<TargetBehavior>();
        move = false;
    }

    public void moveBetweenPoints(List<Vector3> points, int targetMovementSpeed)
    {
        move = false;
        movementPoints = points;
        transform.localPosition = movementPoints[0];
        cur_index = 1;

        speed = step * targetMovementSpeed;
    }

    public void enableMovement()
    {
        move = true;
    }

    void Update()
    {
        if (move && !tb.getIsDown())
        {
            if (movementPoints.Count > 1)
            {
                Vector3 destination = movementPoints[cur_index];

                transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, speed);

                if (Vector3.Distance(transform.localPosition, destination) < 0.001f)
                {
                    cur_index = (cur_index + 1) % movementPoints.Count;
                }
            }
        }
    }

}
