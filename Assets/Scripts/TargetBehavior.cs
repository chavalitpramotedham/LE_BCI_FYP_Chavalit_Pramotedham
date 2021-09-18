using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    private GameObject gameManager;

    public GameObject target_1;
    public GameObject target_2;
    public GameObject target_3;
    public GameObject target_4;

    private List<GameObject> targets = new List<GameObject>();

    private float range_target_1 = 0.25f;
    private float range_target_2 = 0.5f;
    private float range_target_3 = 0.75f;
    private float range_target_4 = 1f;

    private float range_scale = 1.5f;

    private bool isDown;
    private bool? isHit;

    private Rigidbody rb;
    private Collider col;

    private Quaternion originalRotation;

    // Start is called before the first frame update
    void Awake()
    {
        targets.Add(target_1);
        targets.Add(target_2);
        targets.Add(target_3);
        targets.Add(target_4);

        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<BoxCollider>();

        originalRotation = transform.rotation;

        isDown = false;
        isHit = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            rb.isKinematic = false;
            col.isTrigger = false;

            isDown = true;

            rb.velocity += Vector3.forward * 2 + other.gameObject.GetComponent<Rigidbody>().velocity/10;
            other.gameObject.GetComponent<Rigidbody>().velocity = -2 * rb.velocity;

            float distance = calculateDistance(other.GetComponent<DrawTrajectory>().getHitPoint());

            registerPoints(distance);

            StartCoroutine("Countdown");
        }
    }

    private IEnumerator Countdown()
    {
        float duration = 2f; // 3 seconds you can change this 
                             //to whatever you want
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        isHit = true;
    }

    private float calculateDistance(Vector3 otherPosition)
    {
        return Vector3.Distance(transform.position, otherPosition);
    }

    private void registerPoints(float distance)
    {
        int points = 0;

        if (distance <= range_target_1 * range_scale)
        {
            // Target 1:
            points = 50;
            targets[0].GetComponent<TargetHitColor>().hitChangeMaterial();
        }
        else if (distance <= range_target_2 * range_scale)
        {
            points = 35;
            targets[1].GetComponent<TargetHitColor>().hitChangeMaterial();
        }
        else if (distance <= range_target_3 * range_scale)
        {
            points = 20;
            targets[2].GetComponent<TargetHitColor>().hitChangeMaterial();
        }
        else 
        {
            points = 10;
            targets[3].GetComponent<TargetHitColor>().hitChangeMaterial();
        }

        gameManager.GetComponent<PointsManager>().addPoints(points);
    }


    public bool? getIsHit()
    {
        return isHit;
    }

    public bool getIsDown()
    {
        return isDown;
    }

    public void reset()
    {
        isHit = false;
        isDown = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;
        rb.isKinematic = true;
        
        col.enabled = true;
        col.isTrigger = true;

        transform.rotation = originalRotation;

        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].GetComponent<TargetHitColor>().resetMaterial();
        }
    }
}
