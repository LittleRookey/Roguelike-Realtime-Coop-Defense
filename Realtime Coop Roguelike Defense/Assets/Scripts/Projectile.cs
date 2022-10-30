using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 shootDir;
    private float moveSpeed;
    private string enemyTag;
    private float damage;

    public void Setup(Vector3 shootDir, float projectileMoveSpeed, string enemTag)
    {
        this.shootDir = shootDir;
        this.moveSpeed = projectileMoveSpeed;
        this.enemyTag = enemTag;
        
        Destroy(gameObject, 3f);
    }

    public void Setup(Vector3 shootDir, float projectileMoveSpeed, string enemTag, float damage)
    {
        this.shootDir = shootDir;
        this.moveSpeed = projectileMoveSpeed;
        this.enemyTag = enemTag;
        this.damage = damage;

        Destroy(gameObject, 3f);
    }
    private void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(enemyTag))
        {
            Debug.Log("Hit " + collision.tag);
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.GetDamage(this.damage);
            }
            Destroy(gameObject);
        }
    }
}
