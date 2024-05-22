using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;
    public FeetsCollider leftFeet;
    public FeetsCollider rightFeet;

    private Vector2 input;
    private Vector3 displacement;
    public float maxSpeed = 6.0f;
    public float decelerationSpeed = 5.0f;
    public float acelerationForce = 4.0f;
    public float jumpForce = 4.0f;
    private bool jumped = false;
    public bool isGrounded;
    private float landingSpeed;
    public float rotationSpeed = 11.0f;
    public float mouseSensivity = 0.1f;

    private float idleTimer = 0.0f;
    public float idleThreshold = 1.0f;

    public Camera mainCamera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    public float cameraRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main.GetComponent<Camera>();
    }

    private void Update()
    {
        IsGrounded();

        GetCameraDirection();

        GetInput();
    }

    private void OnTriggerEnter(Collider collider)
    {
        //wallContact = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        //wallContact = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        animator.SetFloat("horizontalVelocity", input.magnitude * maxSpeed);

        cameraRotation += Mouse.current.delta.ReadValue().x * mouseSensivity;

        if (isGrounded)
        {
            float jumpInput = playerInput.actions["Jump"].ReadValue<float>();

            jumped = jumpInput > 0.2f;

            if (jumped)
            {
                animator.SetTrigger("jumped");
                animator.ResetTrigger("landed");
            }
        }
    }

    private void Move()
    {
        LookForward();

        AddForces();

        StopInertia();
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (isGrounded)
            {
                //jumped = true;
                //animator.SetTrigger("jumped");
                //Debug.Log("salto");
            }
        }
        Debug.Log(callbackContext.phase);
    }

    private void IsGrounded()
    {
        if (leftFeet.isInTrigger || rightFeet.isInTrigger)
        {
            if (!isGrounded)
            {
                animator.SetTrigger("landed");
                animator.SetFloat("landingSpeed", landingSpeed);
                animator.ResetTrigger("jumped");
                landingSpeed = 0.0f;
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;             

            if (rb.velocity.y < landingSpeed)
            {
                landingSpeed = rb.velocity.y;
            }
        }

        //animator.SetFloat("landingSpeed", landingSpeed);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void GetCameraDirection()
    {
        cameraForward = mainCamera.transform.forward;
        cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
    }

    private void LookForward()
    {
        displacement = input.x * cameraRight + input.y * cameraForward;

        if (displacement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(displacement);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }
    }

    private void AddForces()
    {
        if (isGrounded)
        {
            if (rb.velocity.magnitude <= maxSpeed * 0.5f)
            {
                rb.AddForce(displacement * (maxSpeed * acelerationForce));
            }
            else
            {
                rb.AddForce(displacement * maxSpeed);
            }

            if (jumped)
            {
                rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
                jumped = false;
            }
        }
        else
        {
            rb.AddForce(displacement * 5.0f);
            animator.SetFloat("verticalVelocity", rb.velocity.y);
        }
    }

    private void StopInertia()
    {
        if (rb.velocity != Vector3.zero && input == Vector2.zero && isGrounded)
        {
            Vector3 stop = rb.velocity;
            stop.x = Mathf.Lerp(stop.x, 0, Time.fixedDeltaTime * decelerationSpeed);
            stop.z = Mathf.Lerp(stop.z, 0, Time.fixedDeltaTime * decelerationSpeed);

            rb.velocity = stop;
        }
    }

    public void ControlsChanged(PlayerInput playerInput)
    {
        Debug.Log("Cambio de dispositivo: " + playerInput.currentControlScheme);
    }
}
