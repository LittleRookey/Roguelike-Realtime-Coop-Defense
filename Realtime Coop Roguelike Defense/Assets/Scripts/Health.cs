using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    public bool destroyOnDeath;

    private float _currentHealth;

    public UnityAction<float> OnHit;
    public UnityAction OnDeath;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    public void GetDamage(float dmg)
    {
        _currentHealth -= dmg;
        OnHit?.Invoke(_currentHealth/_maxHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0f;
            OnHit?.Invoke(_currentHealth / _maxHealth);
            OnDeath?.Invoke();
            if (destroyOnDeath) Destroy(gameObject);
        }
    }
}
