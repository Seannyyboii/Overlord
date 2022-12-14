using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : Interactable
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        Debug.Log("Interact with " + gameObject.name);
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        player.GetComponent<InputManager>().enabled = false;

        FindObjectOfType<Timer>().startCountDown = true;

        yield return new WaitForSeconds(2.5f);
        player.GetComponent<InputManager>().enabled = true;
    }
}
