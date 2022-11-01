using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _movementSpeed;
    public bool canMove = true;
    private Vector3 directionVec; // direction player is moving

    public Vector3 DirectionVec => directionVec;

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!canMove) return;
        float _x = Input.GetAxis("Horizontal");
        float _y = Input.GetAxis("Vertical");
        
        if (_x == 0 && _y == 0) return;

        directionVec = new Vector2(_x, _y);
        transform.position += directionVec * _movementSpeed * Time.deltaTime;

    }
}
