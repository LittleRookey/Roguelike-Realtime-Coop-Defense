using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] public Animator anim;
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private UnitAttack playerAttack;
    Vector3 movementVector;

    private readonly int isRun = Animator.StringToHash("isRun");
    private readonly int attack = Animator.StringToHash("attack");
    private readonly int isLookUp = Animator.StringToHash("isLookUp");

    private void Awake()
    {
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
        if (playerAttack == null)
            playerAttack = GetComponent<UnitAttack>();
    }

    private void OnEnable()
    {
        playerAttack.OnAttack += RunAttackAnim;
    }

    private void OnDisable()
    {
        playerAttack.OnAttack -= RunAttackAnim;
    }

    private void RunAttackAnim(GameObject go)
    {
        anim.SetTrigger(attack);
    }

    // Update is called once per frame
    void Update()
    {
        movementVector = playerMovement.DirectionVec;
        if (movementVector.x == 0 && movementVector.y == 0)
        {
            anim.SetBool(isRun, false);
            return;
        }

        anim.SetBool(isRun, true);
        anim.SetFloat("VelocityX", movementVector.x);
    }

    public void LookUp(bool isTrue)
    {
        anim.SetBool(isLookUp, isTrue);
    }
}