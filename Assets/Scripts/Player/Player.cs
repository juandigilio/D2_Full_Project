using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
    private Rigidbody rigiboy;
    private MovementBehaviour movementBehaviour;
    private JumpBehaviour jumpBehaviour;
    private PrayBehaviour prayBehaviour;

    public Vector2 stickInput;
    public Vector2 input;

    private bool isAnimating;

    /// <summary>
    /// Initializes behaviors and subscribes to events.
    /// </summary>
    private void Awake()
    {
        movementBehaviour = GetComponent<MovementBehaviour>();
        jumpBehaviour = GetComponent<JumpBehaviour>();
        prayBehaviour = GetComponent<PrayBehaviour>();

        isAnimating = false;
        Altar.OnPlayerPause += StopMoving;
    }

    /// <summary>
    /// Updates player input.
    /// </summary>
    private void Update()
    {
        GetInput();
    }

    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    private void OnDestroy()
    {
        Altar.OnPlayerPause -= StopMoving;
    }

    /// <summary>
    /// Retrieves input values.
    /// </summary>
    public void GetInput()
    {
        if (isAnimating)
        {
            input = Vector2.zero;
        }
        else
        {
            input = stickInput;
        }
    }

    /// <summary>
    /// Stops player movement.
    /// </summary>
    private void StopMoving()
    {
        input = Vector2.zero;
    }

    /// <summary>
    /// Checks if the player is animating.
    /// </summary>
    public bool IsAnimating()
    {
        return isAnimating;
    }

    /// <summary>
    /// Sets the animation state.
    /// </summary>
    public void SetAnimating(bool set)
    {
        isAnimating = set;
    }

    /// <summary>
    /// Marks animation as started.
    /// </summary>
    private void AnimationStarted()
    {
        isAnimating = true;
    }

    /// <summary>
    /// Marks animation as finished.
    /// </summary>
    public void AnimationFinished()
    {
        isAnimating = false;
    }

    /// <summary>
    /// Retrieves the movement behavior.
    /// </summary>
    public MovementBehaviour MovementBehaviour()
    {
        return movementBehaviour;
    }

    /// <summary>
    /// Retrieves the jump behavior.
    /// </summary>
    public JumpBehaviour JumpBehaviour()
    {
        return jumpBehaviour;
    }

    /// <summary>
    /// Retrieves the pray behavior.
    /// </summary>
    public PrayBehaviour PrayBehaviour()
    {
        return prayBehaviour;
    }
}
