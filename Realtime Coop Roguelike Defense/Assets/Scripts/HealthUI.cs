using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum HealthSpawnPos
{
    Up,
    Down
}
public class HealthUI : MonoBehaviour
{
    public Image healthBar;
    Health health;
    private HealthSpawnPos currentSpawnPos = HealthSpawnPos.Down;

    Vector3 upVector;
    Vector3 downVector;
    
    private void Awake()
    {
        transform.parent.TryGetComponent<Health>(out health);
        upVector = new Vector3(0f, 1.7f, 0f);
        downVector = new Vector3(0f, 0f, 0f);
    }
    private void OnEnable()
    {
        health.OnHit += UpdateHealth;
        SetHealthBarPos(currentSpawnPos, Vector3.zero);
    }

    private void OnDisable()
    {
        health.OnHit -= UpdateHealth;
    }

    public void UpdateHealth(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }

    public void SetHealthBarPos(HealthSpawnPos hsp, Vector3 add)
    {
        switch(hsp)
        {
            case HealthSpawnPos.Up:
                currentSpawnPos = HealthSpawnPos.Up;
                transform.localPosition = upVector + add;
                break;
            case HealthSpawnPos.Down:
                currentSpawnPos = HealthSpawnPos.Down;
                transform.localPosition = downVector + add;
                break;
        }
    }
}
