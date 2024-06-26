using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public Player player;
    public PauseManager pauseManager;
    private float mouseSensivity = 0.1f;
    public float cameraRotation = 0;

    public static InputManager instance;

    private void Awake()
    {
        instance = this;

        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerInput.ActivateInput();

        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("Player");

            playerInput.currentActionMap.FindAction("Jump").started += player.Jump;
            playerInput.currentActionMap.FindAction("Pause").started += player.Pause;
            playerInput.currentActionMap.FindAction("Move").started += Move;
            playerInput.currentActionMap.FindAction("Move").performed += Move;
            playerInput.currentActionMap.FindAction("Move").canceled += Move;

            playerInput.SwitchCurrentActionMap("Paused");

            playerInput.currentActionMap.FindAction("Resume").started += pauseManager.Resume;
            playerInput.currentActionMap.FindAction("Up").started += pauseManager.Up;
            playerInput.currentActionMap.FindAction("Down").started += pauseManager.Down;
            playerInput.currentActionMap.FindAction("Select").started += pauseManager.Select;

            playerInput.SwitchCurrentActionMap("Player");
        }
    }

    private void Update()
    {
        if (pauseManager.gameIsPaused && playerInput.currentActionMap.name != "Paused")
        {
            playerInput.SwitchCurrentActionMap("Paused");
        }

        if (!pauseManager.gameIsPaused && playerInput.currentActionMap.name != "Player")
        {
            playerInput.SwitchCurrentActionMap("Player");
        }

        CheckInput();
        MoveCamera();
    }

    public void Unsuscribe()
    {
        playerInput.SwitchCurrentActionMap("Player");
        playerInput.currentActionMap.FindAction("Jump").started -= player.Jump;
        playerInput.currentActionMap.FindAction("Pause").started -= player.Pause;
        playerInput.currentActionMap.FindAction("Move").started -= Move;
        playerInput.currentActionMap.FindAction("Move").performed -= Move;
        playerInput.currentActionMap.FindAction("Move").canceled -= Move;

        playerInput.SwitchCurrentActionMap("Paused");
        playerInput.currentActionMap.FindAction("Resume").started -= pauseManager.Resume;
        playerInput.currentActionMap.FindAction("Up").started -= pauseManager.Up;
        playerInput.currentActionMap.FindAction("Down").started -= pauseManager.Down;
        playerInput.currentActionMap.FindAction("Select").started -= pauseManager.Select;
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void Move(InputAction.CallbackContext callbackContext)
    {
        //Debug.Log("Move");

        if (callbackContext.started)
        {
            player.stickInput = callbackContext.ReadValue<Vector2>();

            Debug.Log( "Vector " + callbackContext.ReadValue<Vector2>());
        }
        if (callbackContext.performed)
        {
            player.stickInput = callbackContext.ReadValue<Vector2>();

            Debug.Log("Performed!!!!! " + callbackContext.ReadValue<Vector2>());
        }
        if (callbackContext.canceled)
        {
            player.stickInput = Vector2.zero;

            //Debug.Log("Performed!!!!! " + callbackContext.ReadValue<Vector2>());
        }
    }

    public void MoveCamera()
    {
        if (!pauseManager.gameIsPaused)
        {
            cameraRotation += Mouse.current.delta.ReadValue().x * mouseSensivity;
        }
    }

    public void CheckInput()
    {
        if (playerInput.currentControlScheme == "Gamepad")
        {
            pauseManager.isJoystick = true;
        }
        else
        {
            pauseManager.isJoystick = false;
        }
    }
}
