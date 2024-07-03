using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    private Player player;
    private Rigidbody rb;
    [SerializeField] private FeetsCollider leftFeet;
    [SerializeField] private FeetsCollider rightFeet;

    private Camera mainCamera;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private float rotationSpeed = 11.0f;
   

    public float deltaTime;

    private Vector3 displacement;
    private Vector3 stopedVelocity;
    [SerializeField] private float maxSpeed = 6.0f;
    [SerializeField] private float decelerationSpeed = 3.0f;
    [SerializeField] private float acelerationForce = 4.0f;
    [SerializeField] private float airSpeedMultiplier = 100.0f;
    [SerializeField] private bool isLanding = false;
    [SerializeField] private bool badLanded;
    [SerializeField] private bool isStuck;
    [SerializeField] private float rigibodySpeed;
    private bool isGrounded;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = player.GetComponent<Rigidbody>();
        mainCamera = Camera.main.GetComponent<Camera>();
    }

    private void Update()
    {
        GetCameraDirection();

        CheckGround();

        CheckIfStuck();
    }

    private void FixedUpdate()
    {
        UpdateDelta();
        rigibodySpeed = rb.velocity.magnitude;
    }

    public void Move()
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
            Debug.Log("feet trigger");
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


    /// <summary>
    /// Desacelerate player horizontal direction when no input is not pressed
    /// </summary>
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

    public void IsLanding(bool set)
    {
        isLanding = set;
    }

    public bool IsLanding()
    {
        return isLanding;
    }

    public void BadLanded(bool set)
    {
        badLanded = set;
    }


    /// <summary>
    /// returns player position.y 
    /// </summary>
    /// <returns></returns>
    public float PosY()
    {
        return rb.transform.position.y;
    }

    public Vector3 RbVelocity()
    {
        return rb.velocity;
    }

    public Rigidbody PlayerRb()
    {
        return rb;
    }

    private void UpdateDelta()
    {
        deltaTime = Time.fixedDeltaTime;
    }
}
