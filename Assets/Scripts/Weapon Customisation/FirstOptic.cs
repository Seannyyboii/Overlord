
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstOptic : CustomisationBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetMesh.name == "Elcan SpecterDR")
        {
            FindObjectOfType<PlayerController>().primaryAimPosition = new Vector3(0f, 0.104f, 0.4f);
            FindObjectOfType<PlayerController>().primaryAimRotation = Quaternion.Euler(0, 0, -9.825f);
        }
        else if (targetMesh.name == "No First Optic" || targetMesh.name == "EOTech_Mesh" || targetMesh.name == "Hybrid")
        {
            FindObjectOfType<PlayerController>().primaryAimPosition = new Vector3(0f, 0.098f, 0.4f);
            FindObjectOfType<PlayerController>().primaryAimRotation = Quaternion.Euler(0, 0, -9.825f);
        }
        else if(targetMesh.name == "WaltherMRS" || targetMesh.name == "pbr_mini_red_dot" || targetMesh.name == "SightLP")
        {
            FindObjectOfType<PlayerController>().primaryAimPosition = new Vector3(0f, 0.1125f, 0.4f);
            FindObjectOfType<PlayerController>().primaryAimRotation = Quaternion.Euler(0, 0, -9.825f);
        }
        else if (targetMesh.name == "VortexSpitfire")
        {
            FindObjectOfType<PlayerController>().primaryAimPosition = new Vector3(0f, 0.087f, 0.4f);
            FindObjectOfType<PlayerController>().primaryAimRotation = Quaternion.Euler(0, 0, -9.825f);
        }
        else if(targetMesh.name == "SFO Kingslayer")
        {
            FindObjectOfType<PlayerController>().primaryAimPosition = new Vector3(0f, 0.107f, 0.4f);
            FindObjectOfType<PlayerController>().primaryAimRotation = Quaternion.Euler(0, 0, -9.825f);
        }
        else if (targetMesh.name == "OKP_3_LP")
        {
            FindObjectOfType<PlayerController>().primaryAimPosition = new Vector3(0f, 0.105f, 0.4f);
            FindObjectOfType<PlayerController>().primaryAimRotation = Quaternion.Euler(0, 0, -9.825f);
        }
        else if (targetMesh.name == "VortexRazor")
        {
            FindObjectOfType<PlayerController>().primaryAimPosition = new Vector3(0f, 0.08f, 0.4f);
            FindObjectOfType<PlayerController>().primaryAimRotation = Quaternion.Euler(0, 0, -9.825f);
        }
        else if (targetMesh.name == "mini_red_dot")
        {
            FindObjectOfType<PlayerController>().secondaryAimPosition = new Vector3(0.041f, 0.16f, 0.45f);
            FindObjectOfType<PlayerController>().secondaryAimRotation = Quaternion.Euler(0, 0, -10.95f);
        }
        else
        {
            FindObjectOfType<PlayerController>().primaryAimPosition = new Vector3(0f, 0.093f, 0.4f);
            FindObjectOfType<PlayerController>().primaryAimRotation = Quaternion.Euler(0, 0, -9.825f);

            FindObjectOfType<PlayerController>().secondaryAimPosition = new Vector3(0.041f, 0.172f, 0.45f);
            FindObjectOfType<PlayerController>().secondaryAimRotation = Quaternion.Euler(0, 0, -10.95f);
        }
    }
}
