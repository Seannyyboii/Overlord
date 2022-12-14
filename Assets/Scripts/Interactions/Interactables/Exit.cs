using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : Interactable
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        Debug.Log("Interact with " + gameObject.name);
        SceneManager.LoadScene("TestScene");
    }
}
