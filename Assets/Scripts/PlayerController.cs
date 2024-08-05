using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    Transform cam;

    public float speed = 8f;
    public float crouchSpeed = 5f;
    public float turnSmoothTime = 0.4f;
    bool crouched = false;


    float turnSmoothVelocity;

    [SerializeField] private GameObject weaponPoint;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            crouched = true;
            //anim.SetBool("Crouch", true);
            transform.localScale = new Vector3(0f, 0.6f, 0f);
        }
        else
        {
            crouched = false;
            //anim.SetBool("Crouch", false);
            transform.localScale = new Vector3(0f, 1f, 0f);

        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (crouched)
            {
                controller.Move(moveDir.normalized * crouchSpeed * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            //anim.SetBool("Walk", true);
        }
        else
        {
            //anim.SetBool("Walk", false);
        }
    }
}
