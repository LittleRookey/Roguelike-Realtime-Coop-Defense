using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 shootDir;
    private float moveSpeed;

    public void Setup(Vector3 shootDir, float projectileMoveSpeed)
    {
        this.shootDir = shootDir;
        this.moveSpeed = projectileMoveSpeed;

        
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            Destroy(gameObject);
        }
    }
}
