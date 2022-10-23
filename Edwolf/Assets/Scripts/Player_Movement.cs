using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    Rigidbody rigibody;
    [SerializeField] float MoveSpeedHorizontal = 5f;
    [SerializeField] float MoveSpeedVertical = 5f;
    [SerializeField] float JumpVelocity = 5f;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    private void Start()
    {
        rigibody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rigibody.velocity = new Vector3(horizontalInput * MoveSpeedHorizontal, rigibody.velocity.y, verticalInput * MoveSpeedVertical);

        if (Input.GetKeyDown("space") && GroundTouch() == true)
        {
            rigibody.velocity = new Vector3(rigibody.velocity.x, JumpVelocity, rigibody.velocity.z);
        }

    }

    bool GroundTouch()
    {
       return Physics.CheckSphere(groundCheck.position, 0.1f, ground);
    }
}