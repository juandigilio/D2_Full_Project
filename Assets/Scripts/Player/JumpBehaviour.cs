using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class JumpBehaviour : MonoBehaviour
{
    private MovementBehaviour movementBehaviour;

    [SerializeField] private float jumpHeight = 5.0f;

    public static Action OnPlayerJumped;

    private void Awake()
    {
        movementBehaviour = GetComponent<MovementBehaviour>();
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            if (movementBehaviour.IsGrounded() || movementBehaviour.IsStuck())
            {
                OnPlayerJumped?.Invoke();
                CalculateJump();
            }
        }
    }

    private void CalculateJump()
    {
        float jumpForce = Mathf.Sqrt(jumpHeight * 2 * movementBehaviour.Gravity());
        movementBehaviour.VelocityY(jumpForce);
    }
}
