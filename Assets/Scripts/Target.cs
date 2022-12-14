using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100;
    private GameObject mesh;

    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Die()
    {
        mesh = transform.GetChild(0).gameObject;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        mesh.transform.localPosition = Vector3.Lerp(mesh.transform.localPosition, new Vector3(0, -10, 0), 2 * Time.deltaTime);
    }
}
