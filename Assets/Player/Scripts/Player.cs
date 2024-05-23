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
    private Vector3 stopedVelocity;

    private float deltaTime;
    public float maxSpeed = 6.0f;
    public float decelerationSpeed = 5.0f;
    public float acelerationForce = 4.0f;
    public float jumpForce = 4.0f;
    public float airSpeedMultiplier = 100.0f;
    private bool jumped = false;
    public bool isGrounded;
    private bool isStuck;
    private bool isAnimating;
    private bool badLanded;
    private float landingSpeed;
    public float badLandingLimit = 10.0f;
    public float rotationSpeed = 11.0f;
    public float mouseSensivity = 0.1f;

    private float idleTimer = 0.0f;
    public float idleThreshold = 1.0f;

    public Camera mainCamera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    public float cameraRotation;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main.GetComponent<Camera>();

        isAnimating = false;
    }
    private void OnEnable()
    {
        if(playerInput != null)
        {
            playerInput.currentActionMap.FindAction("Jump").started += Jump;
        }
    }

    private void Update()
    {
        IsGrounded();

        GetCameraDirection();

        GetInput();
    }

    private void FixedUpdate()
    {
        UpdateDelta();

        Move();

        Debug.Log("isAnimating: " + isAnimating);
    }

    private void GetInput()
    {
        if (isAnimating)
        {
            input = Vector2.zero;
        }
        else
        {
            input = playerInput.actions["Move"].ReadValue<Vector2>();
        }

        cameraRotation += Mouse.current.delta.ReadValue().x * mouseSensivity;
    }

    private void Move()
    {
        LookForward();

        AddForces();

        StopInertia();
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            if (isGrounded || isStuck)
            {
                jumped = true;
                animator.SetTrigger("jumped");
                animator.ResetTrigger("landed");
                //Debug.Log("Jumped");
            }
        }
       // Debug.Log(callbackContext.phase);
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

                isAnimating = true;
                
                if (landingSpeed > badLandingLimit)
                {
                    badLanded = true;
                }

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

        isStuck = rb.velocity.magnitude > -0.1f && rb.velocity.magnitude < 0.1f && !isGrounded;

        //Debug.Log("isStuck: " + isStuck);

        animator.SetBool("isStuck", isStuck);

        if (isStuck)
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", isGrounded);
        }
        
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
            if (rb.velocity.magnitude <= maxSpeed * 0.25f)
            {
                rb.AddForce(displacement * (maxSpeed * acelerationForce) * deltaTime);
                //Debug.Log("AcelerationAdded");
            }
            else
            {
                rb.AddForce((displacement * maxSpeed) * deltaTime);
                //Debug.Log("maxSpeed");
            }

            if (jumped)
            {
                rb.AddForce((jumpForce * Vector3.up) * deltaTime, ForceMode.Impulse);

                jumped = false;
            }

            animator.SetFloat("verticalVelocity", 0.0f);
            animator.SetFloat("horizontalVelocity", rb.velocity.magnitude);
        }
        else
        {
            rb.AddForce((displacement * airSpeedMultiplier) * deltaTime);
            animator.SetFloat("verticalVelocity", rb.velocity.y);

            if (rb.velocity.y < -2.0f)
            {
                animator.SetFloat("horizontalVelocity", 0.0f);
            }
        }

        if (badLanded)
        {
            rb.velocity = Vector3.zero;
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

    private void UpdateDelta()
    {
        deltaTime = Time.fixedDeltaTime;
    }

    private void AnimationStarted()
    {
        isAnimating = true;
    }

    private void AnimationFinished()
    {
        isAnimating = false;
    }
}
