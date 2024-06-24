using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public Player player;
    public PauseManager pauseManager;
    private float mouseSensivity = 0.1f;
    public float cameraRotation = 0;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.currentActionMap.FindAction("Jump").started += player.Jump;
            playerInput.currentActionMap.FindAction("Pause").started += player.Pause;
            playerInput.currentActionMap.FindAction("Move").started += Move;

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

    public void Move(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            player.stickInput = playerInput.actions["Move"].ReadValue<Vector2>();
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