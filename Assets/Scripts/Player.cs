using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;

    private Vector2 input;
    private float jumpForce = 250.0f;
    private float speed = 10.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        input =  playerInput.actions["Move"].ReadValue<Vector2>();      
        Debug.Log(input);
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x, 0f, input.y) * speed);
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            rb.AddForce(Vector3.up * jumpForce);   
        }
        Debug.Log(callbackContext.phase);
    }

    public void ControlsChanged(PlayerInput playerInput)
    {
        Debug.Log("Cambio de dispositivo: " + playerInput.currentControlScheme);
    }
}
