using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(menuName ="Litkey/Ability/ForwardShield")]
public class ForwardShield : SpawnCollisionAbility
{
    [Space]
    [Header("Health Settings")]
    [SerializeField] private Vector2 healthBarOffset;
    // how shield works
    // shield handles the damage first
    [SerializeField] private float _shieldAmount;
    Health objectHealth;

    protected override async Task WhilePlayerCantMove(GameObject parent)
    {
        chantDone = await OnChantStart(parent);
    }
    public override async void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
        await WhilePlayerCantMove(parent);
        collisionHandler.gameObject.SetActive(true);
        if (spawnedObject.GetComponent<Health>() == null)
            spawnedObject.AddComponent<Health>();
        objectHealth = spawnedObject.GetComponent<Health>();
        objectHealth.SetScript(_shieldAmount, true, true);
        objectHealth.healthUI.SetHealthBarPos(HealthSpawnPos.Up, healthBarOffset);
    }

    public override void OnAbilityRunning(GameObject parent)
    {
        base.OnAbilityRunning(parent);
    }

    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
    }

    public override void OnTriggerEnteredFunc(Collider2D collision2D)
    {
        base.OnTriggerEnteredFunc(collision2D);
        if (collision2D.gameObject.CompareTag("Projectile"))
        {
            // give effect maybe 
            Projectile pj = collision2D.gameObject.GetComponent<Projectile>();
            if (isPlayer) // if the shield is by player 
            {
                if (pj.enemyTag == "Player")
                {
                    objectHealth.TakeDamage(pj.damage);
                    Destroy(collision2D.gameObject);
                }
            } 
            else
            {
                if (pj.enemyTag == "Enemy")
                {
                    objectHealth.TakeDamage(pj.damage);
                    Destroy(collision2D.gameObject);
                }
            }
        }
    }
}
