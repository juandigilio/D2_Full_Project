using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;

    private Vector2 input;
    private Vector3 displacement;
    private float maxSpeed = 3.0f;
    private float decelerationSpeed = 5.0f;
    private float acelerationForce = 80.0f;
    private float jumpForce = 4.0f;

    private float idleTimer = 0.0f;
    public float idleThreshold = 1.0f;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isIdle = true;
    private int idleState;
    private int jumpState;

    private float cameraRotationX;
    public Camera mainCamera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main.GetComponent<Camera>();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        cameraRotationX += Mouse.current.delta.ReadValue().x * 0.05f;
        transform.rotation = Quaternion.Euler(0, cameraRotationX, 0);

        UpdateAnimationsStates();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            isGrounded = true;
            isJumping = false;
            animator.SetBool("jumped", isJumping);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Floor")
        {
            isGrounded = false;
        }
    }

    private void GetInput()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();

        if (input.y < -0.5f)
        {
            input.y = -0.5f;

            if (input.x < -0.5f)
            {
                input.x = -0.5f;
            }
            else if (input.x > 0.5f)
            {
                input.x = 0.5f;
            }
        }
    }

    private void Move()
    {
        GetCameraDirection();

        if (isGrounded)
        {
            displacement = input.x * cameraRight + input.y * cameraForward;

            rb.AddForce(displacement * acelerationForce);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

            if (rb.velocity != Vector3.zero && input == Vector2.zero)
            {
                Vector3 stop = rb.velocity;
                stop.x = Mathf.Lerp(stop.x, 0, Time.fixedDeltaTime * decelerationSpeed);
                stop.z = Mathf.Lerp(stop.z, 0, Time.fixedDeltaTime * decelerationSpeed);

                rb.velocity = stop;
            }
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                isGrounded = false;
                isJumping = true;
            }
        }
        Debug.Log(callbackContext.phase);
    }

    private void UpdateAnimationsStates()
    {
        RuningAnimationState();

        IdleAnimationState();

        JumpAnimationState();
    }

    private void RuningAnimationState()
    {
        animator.SetFloat("speedY", input.y);
        if (input.y < 0)
        {
            animator.SetFloat("speedX", -input.x);
        }
        else
        {
            animator.SetFloat("speedX", input.x);
        }

        //Debug.Log("speedX = " + input.x);
        //Debug.Log("speedY = " + input.y);
    }

    private void JumpAnimationState()
    {
        if (isJumping)
        {
            animator.SetBool("jumped", isJumping);

            if (input.y < 0.5f)
            {
                jumpState = -1;
            }
            else
            {
                jumpState = 0;
            }

            animator.SetFloat("jumpState", jumpState);
        }
    }

    private void IdleAnimationState()
    {
        if (input == Vector2.zero && !isJumping)
        {
            isIdle = true;
            idleTimer += Time.deltaTime;

            idleState = -1;

            if (idleTimer >= idleThreshold)
            {
                idleState = Random.Range(0, 2);
                idleTimer = 0;
            }

            animator.SetFloat("idleState", idleState);
        }
        else
        {
            isIdle = false;
            idleTimer = 0.0f;
        }
        animator.SetBool("isIdle", isIdle);
        isIdle = false;
        Debug.Log("isIdle = " + isIdle);
    }

    public void ControlsChanged(PlayerInput playerInput)
    {
        Debug.Log("Cambio de dispositivo: " + playerInput.currentControlScheme);
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
}
