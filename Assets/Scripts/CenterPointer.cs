using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPointer : MonoBehaviour
{
    private Collider pointerCollider;
    private Vector3 centerPointerVector;

    // Start is called before the first frame update
    void Start()
    {
        pointerCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        ContactPoint contact = collisionInfo.contacts[0];
        centerPointerVector = contact.point;
    }

    public Vector3 getCenterPointerVector()
    {
        return centerPointerVector;
    }
}
