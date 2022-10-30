using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _movementSpeed;
    [SerializeField] private int playerFaceDirection; // 1 right, -1 left
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float _x = Input.GetAxis("Horizontal");
        float _y = Input.GetAxis("Vertical");
        transform.position += new Vector3(_x, _y, 0) * _movementSpeed * Time.deltaTime;
        // handles face directions right or left
        if (_x > 0f) playerFaceDirection = 1;
        else if (_x < 0f) playerFaceDirection = -1;
    }
}
