using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    private Player player;
    public Rigidbody rb;
    [SerializeField] private FeetsCollider leftFeet;
    [SerializeField] private FeetsCollider rightFeet;

    private Camera mainCamera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private float rotationSpeed = 11.0f;
   

    public float deltaTime;

    private Vector3 displacement;
    private Vector3 stopedVelocity;
    public float maxSpeed = 6.0f;
    private float decelerationSpeed = 3.0f;
    public float acelerationForce = 4.0f;
    
    public float airSpeedMultiplier = 100.0f;
    public bool isLanding = false;
    public bool badLanded;
    private bool isStuck;
    private bool isGrounded;

    private void Awake()
    {
        //DontDestroyOnLoad(this);
        player = GetComponent<Player>();
        rb = player.GetComponent<Rigidbody>();
        mainCamera = Camera.main.GetComponent<Camera>();
    }

    private void Update()
    {
        GetCameraDirection();

        CheckGround();
    }

    private void FixedUpdate()
    {
        UpdateDelta();

        //Move();
    }

    public void Move()
    {
        LookForward();

        CheckIfStuck();

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
        if (isGrounded)
        {
            if (rb.velocity.magnitude <= maxSpeed * 0.25f)
            {
                rb.AddForce(displacement * (maxSpeed * acelerationForce) * deltaTime);
            }
            else
            {
                rb.AddForce((displacement * maxSpeed) * deltaTime);
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

    private void CheckGround()
    {
        if (leftFeet.isInTrigger || rightFeet.isInTrigger)
        {
            if (!isGrounded)
            {
                isLanding = true;
            }

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        //Debug.Log("isGrounded: " + isGrounded);
    }

    private void CheckIfStuck()
    {
        isStuck = rb.velocity.magnitude > -0.1f &&
                        rb.velocity.magnitude < 0.1f &&
                        !isGrounded;
    }

    public bool IsStuck()
    {
        return isStuck;
    }

    private void StopInertia()
    {
        if (rb.velocity != Vector3.zero && player.input == Vector2.zero && isGrounded)
        {
            Vector3 stop = rb.velocity;
            stop.x = Mathf.Lerp(stop.x, 0, Time.fixedDeltaTime * decelerationSpeed);
            stop.z = Mathf.Lerp(stop.z, 0, Time.fixedDeltaTime * decelerationSpeed);

            rb.velocity = stop;
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void UpdateDelta()
    {
        deltaTime = Time.fixedDeltaTime;
    }
}
