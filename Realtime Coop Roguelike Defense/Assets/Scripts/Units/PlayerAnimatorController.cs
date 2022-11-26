using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] public Animator anim;
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;

    Vector3 movementVector;

    private readonly int isRun = Animator.StringToHash("isRun");
    private readonly int isLookUp = Animator.StringToHash("isLookUp");

    private void Awake()
    {
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
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