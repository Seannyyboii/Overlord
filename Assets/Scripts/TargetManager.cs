using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private GameObject[] targets;
    public int targetCount;
    public GameObject EndZone;

    // Start is called before the first frame update
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
    }

    // Update is called once per frame
    void Update()
    {
        if(targetCount >= targets.Length)
        {
            EndZone.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
