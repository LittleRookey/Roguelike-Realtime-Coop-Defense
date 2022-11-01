using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Litkey/Ability/Dash")]
public class DashAbility : Ability
{
    [Header("Dash settings")]
    [SerializeField] private float dashForce;
    private Vector2 normalSpeed;
    Rigidbody2D rb;
    [Range(0f, 1f)]
    [SerializeField] private float t;

    public override void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
        Vector3 dirVec = parent.GetComponent<PlayerMovement>().DirectionVec;
        rb = parent.GetComponent<Rigidbody2D>();
        normalSpeed = rb.velocity;
        rb.AddForce(dashForce * dirVec.normalized, ForceMode2D.Impulse);
    }

    public override void OnAbilityRunning(GameObject parent)
    {
        base.OnAbilityRunning(parent);
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, t);
    }

    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
        rb.velocity = Vector2.zero;

    }
}
