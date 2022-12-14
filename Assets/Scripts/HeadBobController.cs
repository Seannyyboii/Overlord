using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool isEnabled = true;
    [SerializeField, Range(0, 0.1f)] private float bobAmplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float bobFrequency = 10.0f;

    [SerializeField] Transform cam;


    private float toggleSpeed = 3f;
    private Vector3 startPosition;
    private CharacterController charController;

    // Start is called before the first frame update
    void Awake()
    {
        charController = GameObject.Find("Player").GetComponent<CharacterController>();
        startPosition = cam.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;

        CheckForMovement();
        ResetPosition();
    }

    private void PlayMotion(Vector3 motion)
    {
        cam.localPosition += motion;
    }


    private void CheckForMovement()
    {
        float speed = new Vector3(charController.velocity.x, 0, charController.velocity.z).magnitude;

        if (speed <= toggleSpeed) return;
        if (!charController.isGrounded) return;

        PlayMotion(FootStepMotion());
    } 

    private void ResetPosition()
    {
        if (cam.localPosition == startPosition) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPosition, 1 * Time.deltaTime);
    }
    private Vector3 FootStepMotion()
    {
        Vector3 position = Vector3.zero;
        position.y += Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        position.x += Mathf.Cos(Time.time * bobFrequency/2) * bobAmplitude * 2;
        return position;
    }
}
