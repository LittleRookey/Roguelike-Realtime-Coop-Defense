using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public bool isPlayer;
    [SerializeField] protected float _moveSpeed; // AI moving speed
    [SerializeField] protected float rayCheckDist; // distance to check collision with the walls
    public Vector3 DirectionVec => directionVec;
    protected Vector3 directionVec; // direction player is moving
    protected Vector3 moveDir; // AI moving direction
    protected bool isRayOn; // switch to turn ray on
    public bool canMove = true; // 

    protected virtual void Start()
    {
        isRayOn = true;
        ChooseNextDirection();
    }

    protected virtual void Update()
    { 
        transform.position += moveDir * _moveSpeed * Time.deltaTime;
        CheckCollideDirection();
    }

    // TODO Can change the movement into shooting ray into direction and 
    // slerp to that position

    // when AI collides somewhere, chooses random direction and move that way
    // Shoot Ray and check if is hit on left, right, top, bottom. 
    // Change complete opposite direction based on hit
    private void ChooseNextDirection()
    {
        float _newXDir = GetRandomDirectionFloat() * (Random.Range(0, 100f) > 50f ? 1 : -1);
        float _newYDir = GetRandomDirectionFloat() * (Random.Range(0, 100f) > 50f ? 1 : -1);
        moveDir = new Vector2(_newXDir, _newYDir).normalized;
        Debug.Log("Move Dir: " +moveDir);
    }

    private void CheckCollideDirection()
    {
        if (isRayOn)
        {
            CheckCollideByDirection(Vector3.right); // checks right collision
            CheckCollideByDirection(Vector3.up); // checks top collision
            CheckCollideByDirection(Vector3.left); // checks left collision
            CheckCollideByDirection(Vector3.down); // checks down collision
        }
    }

    private void CheckCollideByDirection(Vector3 dir)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dir, rayCheckDist, LayerMask.GetMask("BoundryWall"));
        Debug.DrawLine(transform.position, transform.position + dir * rayCheckDist, Color.blue);
        if (hit2D.collider != null)
        {
            Debug.Log(hit2D.collider.name);
            if (dir == Vector3.right)
            {
                moveDir.x = GetRandomDirectionFloat() * -1f;
            } 
            else if (dir == Vector3.up)
            {
                moveDir.y = GetRandomDirectionFloat() * -1f;
            }
            else if (dir == Vector3.left)
            {
                moveDir.x = GetRandomDirectionFloat() * 1f;
            }
            else if (dir == Vector3.down)
            {
                moveDir.y = GetRandomDirectionFloat() * 1f;
            }
            moveDir.Normalize();
            isRayOn = false;
            Invoke("TurnRayOn", .5f);
        }
    }

    // TODO slowly stop and use ability and move again
    private void StopOnceInAWhile()
    {
        moveDir = Vector3.zero;
    }

    private float GetRandomDirectionFloat()
    {
        return Random.Range(0.05f, 1f);
    }

    private void TurnRayOn() => isRayOn = true;
}
