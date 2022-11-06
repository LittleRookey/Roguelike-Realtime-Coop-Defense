using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IHittable
{
    bool TakeDamage(float _amount, bool _isPercent = false, float _armorPenetration = 0f, float _shieldPenetration = 0f);
}

class DamageReceiver : MonoBehaviour, IHittable
{
    private Health health; // GetComponent in Awake or [SerializeField] or whatever
    private Shield shield;
    //private Armour armour;

    private void Awake()
    {
        shield = GetComponent<Shield>();
    }

    public bool TakeDamage(float _amount, bool _isPercent, float _armorPenetration, float _shieldPenetration)
    {
        bool hasDmgLeft = false;
        //if (armour != null)
        //    amount = armour.Remove(amount); // have it return any remainder

        if (shield != null)
        {
            hasDmgLeft = shield.TakeDamage(_amount);
        }

        if (hasDmgLeft) // any left over?
        {
            health.TakeDamage(_amount);

            //if (health.health <= 0)
            //    KillMe();
        }
        throw new System.NotImplementedException();
    }
}
