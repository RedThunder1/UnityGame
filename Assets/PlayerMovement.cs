using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{

    public static CharacterController controller;

    public float baseSpeed = 18f;
    public float speed = 18f;
    public float sprint = 6f;
    Vector3 _velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float crouchSpeedModifier = 2f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Transform headCheck;

    private bool _isGrounded;
    private bool _isCrouched;
    private bool _isJumpColliding;
    private bool isSprinting;
    public static bool _canMove = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        _isJumpColliding = Physics.CheckSphere(headCheck.position, 0.4f, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (!(_isGrounded))
        {
            speed = speed / 2;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (_canMove)
        {
            controller.Move(move * (speed * Time.deltaTime));
        }

        //JUMP CODE


        if (Input.GetButton("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }


        if (Input.GetButtonDown("Crouch"))
        {
            if (_isCrouched)
            {
                controller.transform.localScale += new Vector3(0f, 0.5f, 0f);
                _isCrouched = false;
                speed = (speed * 1.5f);
                jumpHeight = (jumpHeight * 2f);
            }
            else
            {
                controller.transform.localScale -= new Vector3(0f, 0.5f, 0f);
                _isCrouched = true;
                speed = (speed / 1.5f);
                jumpHeight = (jumpHeight / 2f);
            }
        }


        //SPRINT CODE
        if (Input.GetButton("Sprint"))
        {
            isSprinting = true;
            speed = baseSpeed + sprint;
        }
        else if (!(speed.Equals(baseSpeed)))
        {
            isSprinting = false;
            speed = baseSpeed;
        }



        _velocity.y += gravity * Time.deltaTime;
        if (_isCrouched)
        {
            speed = speed / crouchSpeedModifier;
        }
        
        if (_canMove)
        {
            controller.Move(_velocity * Time.deltaTime);

        }
    }
}
