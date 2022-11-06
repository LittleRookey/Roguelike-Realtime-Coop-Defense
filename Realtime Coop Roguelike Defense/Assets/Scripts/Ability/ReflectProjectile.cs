using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Litkey/Ability/ReflectProjectile")]
public class ReflectProjectile : SpawnCollisionAbility
{
    PlayerMovement pm;
    Collider2D[] colliders;
    //[SerializeField] private Vector2 reflectSize;

    public override void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
        pm = parent.GetComponent<PlayerMovement>();
    }

    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
    }

    public override void OnTriggerEnteredFunc(Collider2D collision2D)
    {
        base.OnTriggerEnteredFunc(collision2D);
        CollisionHandle(collision2D);
    }

    private void CollisionHandle(Collider2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Projectile"))
        {
            Projectile pj = collision2D.gameObject.GetComponent<Projectile>();
            if (pj != null)
            {
                collision2D.gameObject.transform.localScale = new Vector3(collision2D.transform.localScale.x * -1, collision2D.transform.localScale.y, collision2D.transform.localScale.z);
                float newX = pj.shootDir.x;
                Vector3 newDir = new Vector3(newX * -1, pj.shootDir.y, 0);
                pj.Setup(newDir, pj.moveSpeed, pj.enemyTag == "Player" ? "Enemy" : "Player");

            }
        }
    }
}
