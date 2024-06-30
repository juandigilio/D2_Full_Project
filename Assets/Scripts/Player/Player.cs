using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;

    public Vector2 stickInput;
    public Vector2 input;

    private bool isAnimating;
    
    private void Awake()
    { 
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

    public void IsAnimating(bool set)
    {
        isAnimating = set;
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
