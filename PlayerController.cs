using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float t, moveH, moveV, mouseX, mouseY, rotX, sensitivity;
    private bool jump, isMoving, sprint, runKey, isGrounded;
    private Rigidbody rb;
    [SerializeField] private int speed, jumpForce;
    [SerializeField] private Transform camHolder, cam;

    void Start()
    {
        Initializations();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        Movement();
        PlayerInputs();
        Camera();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Exp: Prints out what the player is touching (Remove // for execution)
        //Debug.Log("Collision Detected : " + collision.transform.tag);
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void Movement()
    {
        //Script for moving around using rigidbody and physics.
        rb.AddForce(moveV * rb.mass * speed * transform.forward, ForceMode.Force);
        rb.AddForce(moveH * rb.mass * speed * transform.right, ForceMode.Force);

        if (Input.GetKey(key:KeyCode.LeftShift))
        {
            sprint = true;
            speed = 18;
        }
        if (!Input.GetKey(key: KeyCode.LeftShift))
        {
            sprint = false;
            speed = 10;
        }
        if (jump)
        {
            rb.AddForce(transform.up * jumpForce * rb.mass, ForceMode.Impulse);
        }
    }

    private void Camera()
    {
        t = 10;
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -85, 85);

        transform.rotation = Quaternion.Euler(transform.rotation.x, mouseX, transform.rotation.z);
        camHolder.transform.eulerAngles = new Vector3(rotX, mouseX, 0);

        cam.transform.position = Vector3.Lerp(cam.transform.position, camHolder.transform.position, Mathf.SmoothStep(0.0f, 1.0f, t));
        cam.transform.forward = camHolder.forward;
    }

    private void PlayerInputs()
    {
        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");

        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        if (moveH != 0 && moveV != 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        jump = Input.GetKey(KeyCode.Space) && isGrounded;

    }

    private void Initializations()
    {
        //Any settings that need to be automatically assigned at the start of the application is here
        speed = 10;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        jumpForce = 2;
        sensitivity = 5;
    }
}
