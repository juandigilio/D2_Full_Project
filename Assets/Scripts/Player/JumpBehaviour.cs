using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class JumpBehaviour : MonoBehaviour
{
    private MovementBehaviour movementBehaviour;

    [SerializeField] private float jumpHeight = 5.0f;
    private bool jumped = false;
    private bool doubleJumped = false;

    [SerializeField] private float minGroundedTime = 0.3f;
    private float groundTimerCounter = 0;

    public static Action OnPlayerJumped;

    private void Awake()
    {
        movementBehaviour = GetComponent<MovementBehaviour>();
    }

    private void Update()
    {
        if (movementBehaviour.IsGrounded())
        {
            groundTimerCounter += Time.deltaTime;

            if (groundTimerCounter > minGroundedTime)
            {
                jumped = false;
                doubleJumped = false;
            }
            
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            if (movementBehaviour.IsGrounded() && !jumped)
            {
                groundTimerCounter = 0;
                doubleJumped = false;
                OnPlayerJumped?.Invoke();
                CalculateJump();
                jumped = true;
            }
            else if(movementBehaviour.PlayerController().velocity.y > 0 && !doubleJumped)
            {
                CalculateJump();
                doubleJumped = true;
            }
        }
    }


    private void CalculateJump()
    {
        float jumpForce = Mathf.Sqrt(jumpHeight * 2 * movementBehaviour.Gravity());
        movementBehaviour.VelocityY(jumpForce);
    }
}
