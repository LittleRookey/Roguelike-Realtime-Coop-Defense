using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Litkey.Utility;
using UnityEngine.Events;

public class UnitAttack : MonoBehaviour
{
    [Header("Unit Settings")]
    [SerializeField] private float _attackDamage = 10f;
    [SerializeField] public float _attackRange;
    [SerializeField] private float _attackPerTime;

    [Header("Projectile Settings")]
    public bool shootStraight;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private GameObject projectile;

    [Header("Other Settings")]
    public string enemyTag;
    [SerializeField] private GameObject body;

    [SerializeField] private GameObject _target;

    private float currentAttackSpeedTimer = 0f;

    private Vector3 lookDir => body.transform.localScale.x < 0 ? Vector3.left : Vector3.right;

    public UnityAction<GameObject> OnAttack;

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
        var dirToEnemy = (_target.transform.position - (transform.position + lookDir)).normalized;
        if (shootStraight)
            dirToEnemy = lookDir;
        var proj = Instantiate(projectile, transform.position + lookDir, Quaternion.identity);
        proj.transform.rotation = UtilClass.GetRotationFromDirection(dirToEnemy);
        proj.GetComponent<Projectile>().Setup(dirToEnemy, projectileSpeed, enemyTag, _attackDamage);
    }


    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
