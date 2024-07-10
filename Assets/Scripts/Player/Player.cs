using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
    private MovementBehaviour movementBehaviour;
    private JumpBehaviour jumpBehaviour;
    private PrayBehaviour prayBehaviour;

    public Vector2 stickInput;
    public Vector2 input;

    private bool isAnimating;
    private bool cheated = false;

    /// <summary>
    /// Initializes behaviors and subscribes to events.
    /// </summary>
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }

        movementBehaviour = GetComponent<MovementBehaviour>();
        jumpBehaviour = GetComponent<JumpBehaviour>();
        prayBehaviour = GetComponent<PrayBehaviour>();

        isAnimating = false;
        Altar.OnAltarAnimationStarted += StopMoving;
    }

    private void Update()
    {
        //Debug.Log("animating: " + isAnimating);
    }

    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    private void OnDestroy()
    {
        Altar.OnAltarAnimationStarted -= StopMoving;
    }

    /// <summary>
    /// Retrieves input values.
    /// </summary>
    public void GetInput()
    {
        if (isAnimating)
        {
            input = Vector2.zero;
            //Debug.Log("animating: " + isAnimating);
        }
        else
        {
            input = stickInput;
        }
        //Debug.Log("input: " + input);
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

    public void Cheated(bool state)
    {
        cheated = state;
    }

    public bool Cheated()
    {
        return cheated;
    }

    /// <summary>
    /// Marks animation as finished.
    /// </summary>
    public void AnimationFinished()
    {
        isAnimating = false;
        //Debug.Log("animation finished!!!!" + isAnimating);
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
