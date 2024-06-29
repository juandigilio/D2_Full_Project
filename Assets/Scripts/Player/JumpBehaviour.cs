using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpBehaviour : MonoBehaviour
{
    private MovementBehaviour movementBehaviour;

    [SerializeField] private float jumpForce = 600.0f;

    public static Action OnPlayerJumped;

    private void Awake()
    {
        movementBehaviour = GetComponent<MovementBehaviour>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            if (movementBehaviour.IsGrounded() || movementBehaviour.IsStuck())
            {
                OnPlayerJumped?.Invoke();
                movementBehaviour.rb.AddForce((jumpForce * Vector3.up) * movementBehaviour.deltaTime, ForceMode.Impulse);
            }
        }
    }
}
