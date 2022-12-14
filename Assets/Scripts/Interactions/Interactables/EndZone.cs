using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    private Timer timer;
    public GameObject StartZone;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(ResetCourseTimer());
    }

    void ResetCourse()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        GameObject mesh;

        FindObjectOfType<TargetManager>().targetCount = 0;
        GetComponent<BoxCollider>().isTrigger = false;
        
        foreach (GameObject target in targets)
        {
            if(target.TryGetComponent<Target>(out Target hit))
            {
                hit.health = 100;
            }
            mesh = target.transform.GetChild(0).gameObject;
            target.GetComponent<BoxCollider>().enabled = true;
            mesh.transform.localPosition = new Vector3(0, 9.735712f, 0);
        }
    }


    IEnumerator ResetCourseTimer()
    {
        timer.ResetTimer();

        yield return new WaitForSeconds(1);

        StartZone.GetComponent<BoxCollider>().enabled = true;
        ResetCourse();
    }
}
