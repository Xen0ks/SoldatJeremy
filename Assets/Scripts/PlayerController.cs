using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    Transform cam;

    public float speed = 6f;
    public float turnSmoothTime = 0.4f;
    bool fpsv = false;


    float turnSmoothVelocity;

    [SerializeField] private Animator anim;
    [SerializeField] private Transform headPoint;
    [SerializeField] private GameObject gfx;
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
            speed = 4;
            anim.SetBool("Crouch", true);
            headPoint.localPosition = new Vector3(0, 2.5f, 0.5f);
        }
        else
        {
            speed = 6;
            anim.SetBool("Crouch", false);
            headPoint.localPosition = new Vector3(0, 3, 0.5f);
        }

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if(!fpsv) transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeView();
        }
    }

    void ChangeView()
    {
        if (fpsv)
        {
            cam.GetComponent<CinemachineBrain>().enabled = true;
            cam.GetComponent<MouseLook>().enabled = false;
            gfx.SetActive(true);
            weaponPoint.SetActive(false);
            fpsv = false;
        }
        else
        {
            cam.GetComponent<CinemachineBrain>().enabled = false;
            cam.GetComponent<MouseLook>().enabled = true;
            cam.position = headPoint.position;
            gfx.SetActive(false);
            weaponPoint.SetActive(true);
            fpsv = true;
        }
    }
}
