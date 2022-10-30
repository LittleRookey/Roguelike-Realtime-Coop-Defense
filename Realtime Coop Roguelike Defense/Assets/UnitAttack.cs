using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Litkey.Utility;

public class UnitAttack : MonoBehaviour
{
    [Header("Unit Settings")]
    [SerializeField] public float _attackRange;
    [SerializeField] private float _attackPerTime;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private GameObject projectile;

    [SerializeField] private GameObject _target;

    private float currentAttackSpeedTimer = 0f;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentAttackSpeedTimer -= Time.deltaTime;
         if (currentAttackSpeedTimer <= 0 && _target != null)
        {
            // attack here
            currentAttackSpeedTimer = _attackPerTime;
            DoAttack();
        }
    }

    private void DoAttack()
    {
        var dirToEnemy = (_target.transform.position - (transform.position + Vector3.right));

        var proj = Instantiate(projectile, transform.position + Vector3.right, Quaternion.identity);
        proj.transform.rotation = UtilClass.GetRotationFromDirection(dirToEnemy);
        proj.GetComponent<Projectile>().Setup(dirToEnemy, projectileSpeed);
    }


    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
