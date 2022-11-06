using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 shootDir { private set; get; }
    public float moveSpeed { private set; get; }
    public string enemyTag { private set; get; }
    public float damage { private set; get; }

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

    public ProjectileSettings GetProjectileSettings()
    {
        return new ProjectileSettings(shootDir, moveSpeed, enemyTag, damage);
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
                enemyHealth.TakeDamage(this.damage);
            }
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class ProjectileSettings
{
    public Vector3 shootDir { private set; get; }
    public float moveSpeed { private set; get; }
    public string enemyTag { private set; get; }
    public float damage { private set; get; }

    public ProjectileSettings(Vector3 shootDir, float moveSpeed, string enemyTag, float damage)
    {
        this.shootDir = shootDir;
        this.moveSpeed = moveSpeed;
        this.enemyTag = enemyTag;
        this.damage = damage;
    }
}
