using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    private Player player;
    private CharacterController controller;
    [SerializeField] private Transform feetsPosition;
    private Camera mainCamera;

    private Vector3 cameraForward;
    private Vector3 cameraRight;
    private float rotationSpeed = 11.0f;


    public float deltaTime;

    private Vector3 displacement;
    private Vector3 velocity;

    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float gravity = 4.8f;
    [SerializeField] private float maxFallingSpeed = 10f;
    [SerializeField] private float decelerationSpeed = 3.0f;
    [SerializeField] private float accelerationForce = 4.0f;
   // [SerializeField] private float airSpeedMultiplier = 100.0f;
    [SerializeField] private bool isLanding = false;
    [SerializeField] private bool badLanded;
    [SerializeField] private bool isStuck;
    private bool isGrounded = true;

    private void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController>();
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

        UpdateGravity();
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

        //Debug.Log("displacement " + displacement);

        if (displacement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(displacement);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void AddForces()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        Vector3 targetVelocity = displacement * maxSpeed;

        if (player.input != Vector2.zero)
        {
            displacement = Vector3.Lerp(displacement, targetVelocity, accelerationForce * Time.deltaTime);
        }

        controller.Move(displacement * Time.deltaTime);

        if (badLanded)
        {
            displacement = Vector3.zero;
        }

        velocity.y -= gravity * Time.deltaTime;

        if (velocity.y < -maxFallingSpeed)
        {
            velocity.y = -maxFallingSpeed;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateGravity()
    {
        velocity.y -= gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void CheckGround()
    {
        bool isTouchingFloor = Physics.CheckSphere(feetsPosition.position, groundDistance);

        if (isTouchingFloor && !isGrounded)
        {
            isLanding = true;
        }

        isGrounded = isTouchingFloor;

        //Debug.Log("isGounded check " + isGrounded);
    }

    private void CheckIfStuck()
    {
        isStuck = velocity.magnitude > -0.1f &&
                        velocity.magnitude < 0.1f &&
                        !isGrounded;
    }

    public void StopInertia()
    {
        if (player.input == Vector2.zero)
        {
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, decelerationSpeed * Time.deltaTime);
            velocity.x = horizontalVelocity.x;
            velocity.z = horizontalVelocity.z;
        }
    }

    public bool IsStuck()
    {
        return isStuck;
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
        return transform.position.y;
    }

    public Vector3 Displacement()
    {
        return displacement;
    }

    public Vector3 Velocity()
    {
        return velocity;
    }

    public void VelocityY(float set)
    {
        velocity.y = set;
    }

    public float Gravity()
    {
        return gravity;
    }

    public CharacterController PlayerController()
    {
        return controller;
    }

    private void UpdateDelta()
    {
        deltaTime = Time.fixedDeltaTime;
    }
}
