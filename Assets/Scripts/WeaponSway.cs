using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    public void WeaponSwaying(Vector2 input)
    {
        // get mouse input
        float mouseX = input.x * swayMultiplier;
        float mouseY = input.y * swayMultiplier;
        
        // calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // rotate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
