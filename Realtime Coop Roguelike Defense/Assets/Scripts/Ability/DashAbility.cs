using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(menuName ="Litkey/Ability/Dash")]
public class DashAbility : Ability
{
    [Header("Dash settings")]
    [SerializeField] private float dashForce;
    private Vector2 normalSpeed;
    Rigidbody2D rb;
    
    [SerializeField] private float speed;

    public override async void OnAbilityStart(GameObject parent)
    {
        base.OnAbilityStart(parent);
        chantDone = await OnChantStart(parent);

        Vector3 dirVec = parent.GetComponent<PlayerMovement>().DirectionVec;
        rb = parent.GetComponent<Rigidbody2D>();
        normalSpeed = rb.velocity;
        rb.AddForce(dashForce * dirVec.normalized, ForceMode2D.Impulse);
    }

    public override void OnAbilityRunning(GameObject parent)
    {
        base.OnAbilityRunning(parent);
        //float k = 1.0f - Mathf.Pow(t, Time.deltaTime);
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, speed * Time.deltaTime);
    }

    public override void OnAbilityEnd(GameObject parent)
    {
        base.OnAbilityEnd(parent);
        rb.velocity = Vector2.zero;

    }
}
