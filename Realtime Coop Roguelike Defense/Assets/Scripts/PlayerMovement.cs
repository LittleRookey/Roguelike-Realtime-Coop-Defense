using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : AIMovement
{

    protected override void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!canMove) return;
        float _x = Input.GetAxis("Horizontal");
        float _y = Input.GetAxis("Vertical");
        
        if (_x == 0 && _y == 0) return;

        directionVec = new Vector2(_x, _y).normalized;
        transform.position += directionVec * _moveSpeed * Time.deltaTime;

    }
}
