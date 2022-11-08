using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : AIMovement
{
    [SerializeField] private Rigidbody2D rb;
    Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!canMove) return;
        float _x = Input.GetAxisRaw("Horizontal");
        float _y = Input.GetAxisRaw("Vertical");

        if (_x == 0 && _y == 0) 
        {
            anim.SetBool("isRun", false);
            return;
            
        }
        anim.SetBool("isRun", true);
        directionVec = new Vector2(_x, _y).normalized;
        anim.SetFloat("VelocityX", directionVec.x);
        transform.localPosition += directionVec * _moveSpeed * Time.deltaTime;

    }
}
