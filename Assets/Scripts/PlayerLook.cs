using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [HideInInspector]
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    // Start is called before the first frame update

    private void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ProcessLook(Vector2 input)
    {
        if (!FindObjectOfType<PlayerController>().inspecting)
        {
            float mouseX = input.x;
            float mouseY = input.y;
            // calculate camera rotation for looking up and down
            xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            // apply this to the camera transform
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            // rotate the player to look left and right
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
    }
}
