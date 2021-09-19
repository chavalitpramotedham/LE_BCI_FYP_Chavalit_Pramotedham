using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class BallLaunch : MonoBehaviour
{
    public GameObject targetSurface;
    public GameObject centerPointer;

    private Vector3 originalBallPos;
    private Vector3 centerPointerPos;

    private Rigidbody rb;

    private bool isShoot = false;
    private float forceMultiplier = 100;

    public bool toShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalBallPos = gameObject.transform.position;
        toShoot = false;

        Physics.IgnoreCollision(centerPointer.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void Update()
    {
        
        centerPointerPos = centerPointer.GetComponent<CenterPointer>().getCenterPointerVector();
        centerPointerPos.y += 1;

        Vector3 forceInit = (centerPointerPos - originalBallPos);
        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.z)) * forceMultiplier;

        if (!isShoot)
        {
            DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);
        }

        if (toShoot)
        {
            toShoot = false;
            StartCoroutine(Shoot(forceV));
        }
    }

    private IEnumerator Shoot(Vector3 ForceV)
    {
        
        if (isShoot)
        {
            print("Failed");
        }
        else
        {
            isShoot = true;
            yield return new WaitForSeconds(1.4f);

            print("Shooting");
            DrawTrajectory.Instance.HideLine();

            rb.AddForce(ForceV);

            StartCoroutine("Countdown");
        }
    }

    private IEnumerator Countdown()
    {
        float duration = 2.5f;

        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }

        resetBallPos();
    }

    private void resetBallPos()
    {
        gameObject.transform.position = originalBallPos;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isShoot = false;
        toShoot = false;
    }

    public void setToShoot()
    {
        toShoot = true;
    }
}
