using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodLightSingleSideController : MonoBehaviour
{
    public Material inactive;
    public Material green;
    public Material red;

    public void setInactive()
    {
        foreach (Transform child in transform)
        {
            MeshRenderer mr = child.GetComponent<MeshRenderer>();

            Material[] materials = mr.materials;
            materials[0] = inactive;
            mr.materials = materials;
        }
    }

    public void setGreen()
    {
        foreach (Transform child in transform)
        {
            MeshRenderer mr = child.GetComponent<MeshRenderer>();

            Material[] materials = mr.materials;
            materials[0] = green;
            mr.materials = materials;
        }
    }

    public void setRed()
    {
        foreach (Transform child in transform)
        {
            MeshRenderer mr = child.GetComponent<MeshRenderer>();

            Material[] materials = mr.materials;
            materials[0] = red;
            mr.materials = materials;
        }
    }
}
