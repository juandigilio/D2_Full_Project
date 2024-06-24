using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    private Player player;
    public Rigidbody rb;

    private Camera mainCamera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private float rotationSpeed = 11.0f;
   

    private float deltaTime;

    private Vector3 displacement;
    private Vector3 stopedVelocity;
    public float maxSpeed = 6.0f;
    public float decelerationSpeed = 5.0f;
    public float acelerationForce = 4.0f;
    public float jumpForce = 4.0f;
    public float airSpeedMultiplier = 100.0f;
    public bool isLanding = false;
    public bool badLanded;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main.GetComponent<Camera>();
    }

    private void Update()
    {
        GetCameraDirection();

        IsGrounded();
    }

    private void FixedUpdate()
    {
        UpdateDelta();

        Move();
    }

    private void Move()
    {
        

        LookForward();

        AddForces();

        StopInertia();
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
        displacement = player.input.x * cameraRight + player.input.y * cameraForward;

        if (displacement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(displacement);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }
    }

    private void AddForces()
    {
        if (player.isGrounded)
        {
            if (rb.velocity.magnitude <= maxSpeed * 0.25f)
            {
                rb.AddForce(displacement * (maxSpeed * acelerationForce) * deltaTime);
            }
            else
            {
                rb.AddForce((displacement * maxSpeed) * deltaTime);
            }

            if (player.jumped)
            {
                rb.AddForce((jumpForce * Vector3.up) * deltaTime, ForceMode.Impulse);

                player.jumped = false;
            }
        }
        else
        {
            rb.AddForce((displacement * airSpeedMultiplier) * deltaTime);

        }

        if (badLanded)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void IsGrounded()
    {
        if (player.leftFeet.isInTrigger || player.rightFeet.isInTrigger)
        {
            if (!player.isGrounded)
            {
                isLanding = true;
            }

            player.isGrounded = true;
        }
        else
        {
            player.isGrounded = false;
        }

        Debug.Log("isGrounded: " + player.isGrounded);
    }

    private void StopInertia()
    {
        if (rb.velocity != Vector3.zero && player.input == Vector2.zero && player.isGrounded)
        {
            Vector3 stop = rb.velocity;
            stop.x = Mathf.Lerp(stop.x, 0, Time.fixedDeltaTime * decelerationSpeed);
            stop.z = Mathf.Lerp(stop.z, 0, Time.fixedDeltaTime * decelerationSpeed);

            rb.velocity = stop;
        }
    }

    private void UpdateDelta()
    {
        deltaTime = Time.fixedDeltaTime;
    }
}
