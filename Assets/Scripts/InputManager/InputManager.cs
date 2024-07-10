using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private JumpBehaviour jumpBehaviour;
    [SerializeField] private PrayBehaviour prayBehaviour;
    [SerializeField] private PauseManager pauseManager;

    private float cameraRotation = 0;
    private float padCameraInput = 0;

    /// <summary>
    /// Player inputs
    /// </summary>
    [SerializeField] private string jumpAction = "Jump";
    [SerializeField] private string prayAction = "Pray";
    [SerializeField] private string pauseAction = "Pause";
    [SerializeField] private string moveAction = "Move";
    [SerializeField] private string rotateCameraAction = "MoveCamera";
    [SerializeField] public string doorAction = "OpenDoor";
    /// <summary>
    /// Paused and menu inputs
    /// </summary>
    [SerializeField] private string resumeAction = "Resume";
    [SerializeField] private string upAction = "Up";
    [SerializeField] private string downAction = "Down";
    [SerializeField] private string selectAction = "Select";
    /// <summary>
    /// Maps
    /// </summary>
    [SerializeField] private string pausedAction = "Paused";
    [SerializeField] public string playerAction = "Player";

    //public static InputManager instance;
    public PlayerInput playerInput { get; private set; }

    public static Action OnOpenDoor;
    public static Action OnKeyboardActive;
    public static Action OnGamepadActive;

    /// <summary>
    /// Sets the instance of InputManager and initializes PlayerInput.
    /// </summary>
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("InputManager");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            //instance = this;
            playerInput = GetComponent<PlayerInput>();
        }
    }

    /// <summary>
    /// Activates player input and assigns input actions.
    /// </summary>
    private void Start()
    {
        playerInput.ActivateInput();

        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap(playerAction);

            playerInput.currentActionMap.FindAction(jumpAction).started += jumpBehaviour.Jump;
            playerInput.currentActionMap.FindAction(prayAction).started += prayBehaviour.Pray;
            playerInput.currentActionMap.FindAction(pauseAction).started += pauseManager.Pause;
            playerInput.currentActionMap.FindAction(moveAction).started += Move;
            playerInput.currentActionMap.FindAction(moveAction).performed += Move;
            playerInput.currentActionMap.FindAction(moveAction).canceled += Move;
            playerInput.currentActionMap.FindAction(rotateCameraAction).started += GamePadCameraRotation;
            playerInput.currentActionMap.FindAction(rotateCameraAction).performed += GamePadCameraRotation;
            playerInput.currentActionMap.FindAction(rotateCameraAction).canceled += GamePadCameraRotation;

            playerInput.SwitchCurrentActionMap(pausedAction);

            playerInput.currentActionMap.FindAction(resumeAction).started += pauseManager.Resume;
            playerInput.currentActionMap.FindAction(upAction).started += pauseManager.Up;
            playerInput.currentActionMap.FindAction(downAction).started += pauseManager.Down;
            playerInput.currentActionMap.FindAction(selectAction).started += pauseManager.Select;

            playerInput.SwitchCurrentActionMap(playerAction);
        }

        CheckInput();
    }

    /// <summary>
    /// Switches action maps based on game state and updates camera position.
    /// </summary>
    private void Update()
    {
        if (pauseManager.GameIsPaused() && playerInput.currentActionMap.name != pausedAction)
        {
            playerInput.SwitchCurrentActionMap(pausedAction);
        }

        if (!pauseManager.GameIsPaused() && playerInput.currentActionMap.name != playerAction)
        {
            playerInput.SwitchCurrentActionMap(playerAction);
        }

        CheckInput();
        MoveCamera();
    }

    /// <summary>
    /// Unsubscribes from all input actions.
    /// </summary>
    public void Unsuscribe()
    {      
        playerInput.SwitchCurrentActionMap(playerAction);
        playerInput.currentActionMap.FindAction(jumpAction).started -= jumpBehaviour.Jump;
        playerInput.currentActionMap.FindAction(prayAction).started -= prayBehaviour.Pray;
        playerInput.currentActionMap.FindAction(moveAction).started -= Move;
        playerInput.currentActionMap.FindAction(moveAction).performed -= Move;
        playerInput.currentActionMap.FindAction(moveAction).canceled -= Move;
        playerInput.currentActionMap.FindAction(rotateCameraAction).started -= GamePadCameraRotation;
        playerInput.currentActionMap.FindAction(rotateCameraAction).performed -= GamePadCameraRotation;
        playerInput.currentActionMap.FindAction(rotateCameraAction).canceled -= GamePadCameraRotation;

        playerInput.SwitchCurrentActionMap(pausedAction);
        playerInput.currentActionMap.FindAction(resumeAction).started -= pauseManager.Resume;
        playerInput.currentActionMap.FindAction(upAction).started -= pauseManager.Up;
        playerInput.currentActionMap.FindAction(downAction).started -= pauseManager.Down;
        playerInput.currentActionMap.FindAction(selectAction).started -= pauseManager.Select;
        playerInput.SwitchCurrentActionMap(playerAction);
    }

    private void OnDestroy()
    {
        //Unsuscribe();
    }

    /// <summary>
    /// Updates player movement input.
    /// </summary>
    public void Move(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            player.stickInput = callbackContext.ReadValue<Vector2>();
        }

        if (callbackContext.performed)
        {
            player.stickInput = callbackContext.ReadValue<Vector2>();
        }

        if (callbackContext.canceled)
        {
            player.stickInput = Vector2.zero;
        }
    }

    private void GamePadCameraRotation(InputAction.CallbackContext callbackContext)
    {
        padCameraInput = callbackContext.ReadValue<Vector2>().x * PlayerConfig.GetPadSensitivity();
    }

    /// <summary>
    /// Update camera rotation value.
    /// </summary>
    private void MoveCamera()
    {
        if (!pauseManager.GameIsPaused())
        {
            if (playerInput.currentControlScheme == "Gamepad")
            {
                if (padCameraInput != 0)
                {
                    cameraRotation += padCameraInput;
                }
            }
            else
            {
                cameraRotation += Mouse.current.delta.ReadValue().x * PlayerConfig.GetMouseSensitivity();
            }
        }
    }

    /// <summary>
    /// Checks the current input scheme.
    /// </summary>
    private void CheckInput()
    {
        if (playerInput.currentControlScheme == "Gamepad")
        {
            pauseManager.IsJoystick(true);
            OnGamepadActive?.Invoke();
        }
        else
        {
            pauseManager.IsJoystick(false);
            OnKeyboardActive?.Invoke();
        }
    }

    /// <summary>
    /// Returns the current camera rotation value.
    /// </summary>
    public float GetCameraRotation()
    {
        return cameraRotation;
    }
}
