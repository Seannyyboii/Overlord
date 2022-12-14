using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCustomisation : MonoBehaviour
{
    public List<GameObject> attachmentPoints;

    // Start is called before the first frame update
    void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckActiveAttachments();
    }

    void CheckActiveAttachments()
    {
        //attachmentPoints.Clear();

        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("AttachmentPoint") && transform.gameObject.activeSelf && attachmentPoints.Count < GameObject.Find("AttachmentSelectors").transform.childCount)
            {
                Debug.Log("Attached");
                attachmentPoints.Add(child.gameObject);
            }
        }

        foreach (GameObject attachmentPoint in FindObjectOfType<WeaponCustomisation>().attachmentPoints)
        {
            if (FindObjectOfType<PlayerController>().inspecting)
            {
                attachmentPoint.GetComponent<Renderer>().enabled = true;
                attachmentPoint.GetComponent<SphereCollider>().enabled = true;
            }
            else
            {
                attachmentPoint.GetComponent<Renderer>().enabled = false;
                attachmentPoint.GetComponent<SphereCollider>().enabled = false;
            }
        }

        for (int i = 0; i < FindObjectOfType<WeaponCustomisation>().attachmentPoints.Count; i++)
        {
            if (FindObjectOfType<WeaponCustomisation>().attachmentPoints[i].name == "SelectCantedOptic")
            {
                Renderer renderer = FindObjectOfType<WeaponCustomisation>().attachmentPoints[i].GetComponent<Renderer>();
                SphereCollider sphere = FindObjectOfType<WeaponCustomisation>().attachmentPoints[i].GetComponent<SphereCollider>();

                if (FindObjectOfType<SecondOptic>().targetMesh.name == "Canted Rail" && FindObjectOfType<PlayerController>().inspecting)
                {
                    renderer.enabled = true;
                    sphere.enabled = true;
                }
                else
                {
                    renderer.enabled = false;
                    sphere.enabled = false;
                }
            }
        }
    }
}
