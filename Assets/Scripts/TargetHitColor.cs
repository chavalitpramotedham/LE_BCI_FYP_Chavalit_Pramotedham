using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitColor : MonoBehaviour
{
    private MeshRenderer mr;

    private Material originalMaterial;
    public Material hitMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        mr = gameObject.GetComponent<MeshRenderer>();
        originalMaterial = mr.materials[0];
    }

    public void hitChangeMaterial()
    {
        Material[] mats = mr.materials;
        mats[0] = hitMaterial;
        mr.materials = mats;
    }

    public void resetMaterial()
    {
        Material[] mats = mr.materials;
        mats[0] = originalMaterial;
        mr.materials = mats;
    }
}
