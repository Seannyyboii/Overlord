using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisationBase : MonoBehaviour
{
    public bool active = false;
    public GameObject targetMesh;
    public float value;

    public void Enable()
    {
        active = true;
        targetMesh.SetActive(true);
    }

    public void Disable()
    {
        active = false;
        targetMesh.SetActive(false);
    }
}
