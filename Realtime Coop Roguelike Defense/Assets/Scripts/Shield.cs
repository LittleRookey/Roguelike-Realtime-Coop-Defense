using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float RemainingDamage { get { return remainingDamage; } private set { remainingDamage = value; } }
    public float GetValue { get { return currentValue; } }

    [SerializeField] private float maxValue;
    private float currentValue;
    private float remainingDamage;

    public bool destroyOnDeath;

    private void Start()
    {
        currentValue = maxValue;
    }

    public bool TakeDamage(float _amount, bool _isPercent = false, float _armorPenetration = 0f, float _shieldPenetration = 0f)
    {
        // No armor or shield penetration calculations yet
        // Skipping percent based damage as well

        RemainingDamage = 0f;

        if (_amount <= 0f || currentValue <= 0f)
        {
            if (destroyOnDeath)
                Destroy(gameObject);

            
            return false;
        }

        currentValue -= _amount;

        if (currentValue <= 0f)
        {
            // depleted
            RemainingDamage = Mathf.Abs(currentValue);
            currentValue = 0f;
        }

        return true;
    }
}
