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
    
    private void Awake()
    {
        movementBehaviour = GetComponent<MovementBehaviour>();
        jumpBehaviour = GetComponent<JumpBehaviour>();
        prayBehaviour = GetComponent<PrayBehaviour>();

        isAnimating = false;
        Altar.OnPlayerPause += StopMoving;
    }

    private void Update()
    {
        GetInput();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExitZone"))
        {

            Debug.Log("Next level");
        }
    }

    private void OnDestroy()
    {
        Altar.OnPlayerPause -= StopMoving;
    }

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

    private void StopMoving()
    {
        input = Vector2.zero;
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }

    public void SetAnimating(bool set)
    {
        isAnimating = set;
    }

    private void AnimationStarted()
    {
        isAnimating = true;
    }

    public void AnimationFinished()
    {
        isAnimating = false;
    }

    public MovementBehaviour MovementBehaviour()
    {
        return movementBehaviour;
    }

    public JumpBehaviour JumpBehaviour()
    {
        return jumpBehaviour;
    }

    public PrayBehaviour PrayBehaviour()
    {
        return prayBehaviour;
    }
}
