using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthBar;
    Health health;

    private void Awake()
    {
        transform.parent.TryGetComponent<Health>(out health);
    }
    private void OnEnable()
    {
        health.OnHit += UpdateHealth;
    }

    private void OnDisable()
    {
        health.OnHit -= UpdateHealth;
    }

    public void UpdateHealth(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }
}
