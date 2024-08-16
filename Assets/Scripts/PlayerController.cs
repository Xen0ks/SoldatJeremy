using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;


    [Header("GroundCheck")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Vector3 groundCheckSize;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        MovementInput();
        grounded = Physics.CheckBox(groundCheck.position, groundCheckSize, Quaternion.identity, whatIsGround);
        
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        SpeedControl();

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovementInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * runSpeed * 10f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * runSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }
        else
        {
            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (flatVel.magnitude > runSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        else
        {
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }


    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Force);
    }

    void ResetJump()
    {
        readyToJump = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
    }
}
