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
                //Debug.Log("velocity = " + movementBehaviour.Velocity());
            }
        }
    }

    private void CalculateJump()
    {
        float jumpForce = (float)Math.Sqrt(jumpHeight * -2 * -9.8f);

        //Debug.Log("jumpForce =" + jumpForce);

        movementBehaviour.VelocityY(jumpForce);
    }
}
