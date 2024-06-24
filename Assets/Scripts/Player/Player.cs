using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{    
    public Animator animator;
    public FeetsCollider leftFeet;
    public FeetsCollider rightFeet;
    public PauseManager pauseManager;

    public Vector2 stickInput;
    public Vector2 input;
     
    public bool jumped = false;
    public bool jumpedAnimation = false;
    public bool isGrounded; 
    public bool isStuck;
    public bool isAnimating;
    

    private void Awake()
    {      
        animator = GetComponent<Animator>();
        isAnimating = false;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Debug.Log("isAnimating: " + isAnimating);
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

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            if (isGrounded || isStuck)
            {
                jumped = true;
                jumpedAnimation = true;
            }
        }
    }

    public void Pause(InputAction.CallbackContext callbackContext)
    {
        if (!pauseManager.gameIsPaused)
        {
            if (callbackContext.started)
            {
                pauseManager.Pause();
            }
        }   
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
