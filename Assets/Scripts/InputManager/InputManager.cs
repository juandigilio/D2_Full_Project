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
    [SerializeField] private string doorAction = "OpenDoor";
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
    [SerializeField] private string playerAction = "Player";

    public static InputManager instance;
    public PlayerInput PlayerInput { get; private set; }

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
            instance = this;
            PlayerInput = GetComponent<PlayerInput>();
        }
    }

    /// <summary>
    /// Activates player input and assigns input actions.
    /// </summary>
    private void Start()
    {
        PlayerInput.ActivateInput();

        if (PlayerInput != null)
        {
            PlayerInput.SwitchCurrentActionMap(playerAction);

            PlayerInput.currentActionMap.FindAction(jumpAction).started += jumpBehaviour.Jump;
            PlayerInput.currentActionMap.FindAction(prayAction).started += prayBehaviour.Pray;
            PlayerInput.currentActionMap.FindAction(pauseAction).started += pauseManager.Pause;
            PlayerInput.currentActionMap.FindAction(moveAction).started += Move;
            PlayerInput.currentActionMap.FindAction(moveAction).performed += Move;
            PlayerInput.currentActionMap.FindAction(moveAction).canceled += Move;
            PlayerInput.currentActionMap.FindAction(rotateCameraAction).started += GamePadCameraRotation;
            PlayerInput.currentActionMap.FindAction(rotateCameraAction).performed += GamePadCameraRotation;
            PlayerInput.currentActionMap.FindAction(rotateCameraAction).canceled += GamePadCameraRotation;
            PlayerInput.currentActionMap.FindAction(doorAction).started += OpenDoor;

            PlayerInput.SwitchCurrentActionMap(pausedAction);

            PlayerInput.currentActionMap.FindAction(resumeAction).started += pauseManager.Resume;
            PlayerInput.currentActionMap.FindAction(upAction).started += pauseManager.Up;
            PlayerInput.currentActionMap.FindAction(downAction).started += pauseManager.Down;
            PlayerInput.currentActionMap.FindAction(selectAction).started += pauseManager.Select;

            PlayerInput.SwitchCurrentActionMap(playerAction);
        }

        CheckInput();
    }

    /// <summary>
    /// Switches action maps based on game state and updates camera position.
    /// </summary>
    private void Update()
    {
        if (pauseManager.GameIsPaused() && PlayerInput.currentActionMap.name != pausedAction)
        {
            PlayerInput.SwitchCurrentActionMap(pausedAction);
        }

        if (!pauseManager.GameIsPaused() && PlayerInput.currentActionMap.name != playerAction)
        {
            PlayerInput.SwitchCurrentActionMap(playerAction);
        }

        CheckInput();
        MoveCamera();
    }

    /// <summary>
    /// Unsubscribes from all input actions.
    /// </summary>
    public void Unsuscribe()
    {      
        PlayerInput.SwitchCurrentActionMap(playerAction);
        PlayerInput.currentActionMap.FindAction(jumpAction).started -= jumpBehaviour.Jump;
        PlayerInput.currentActionMap.FindAction(prayAction).started -= prayBehaviour.Pray;
        PlayerInput.currentActionMap.FindAction(moveAction).started -= Move;
        PlayerInput.currentActionMap.FindAction(moveAction).performed -= Move;
        PlayerInput.currentActionMap.FindAction(moveAction).canceled -= Move;
        PlayerInput.currentActionMap.FindAction(rotateCameraAction).started -= GamePadCameraRotation;
        PlayerInput.currentActionMap.FindAction(rotateCameraAction).performed -= GamePadCameraRotation;
        PlayerInput.currentActionMap.FindAction(rotateCameraAction).canceled -= GamePadCameraRotation;
        PlayerInput.currentActionMap.FindAction(doorAction).started -= OpenDoor;

        PlayerInput.SwitchCurrentActionMap(pausedAction);
        PlayerInput.currentActionMap.FindAction(resumeAction).started -= pauseManager.Resume;
        PlayerInput.currentActionMap.FindAction(upAction).started -= pauseManager.Up;
        PlayerInput.currentActionMap.FindAction(downAction).started -= pauseManager.Down;
        PlayerInput.currentActionMap.FindAction(selectAction).started -= pauseManager.Select;
        PlayerInput.SwitchCurrentActionMap(playerAction);
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
            if (PlayerInput.currentControlScheme == "Gamepad")
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
    /// Invokes the OnOpenDoor action.
    /// </summary>
    private void OpenDoor(InputAction.CallbackContext callbackContext)
    {
        OnOpenDoor?.Invoke();
    }

    /// <summary>
    /// Checks the current input scheme.
    /// </summary>
    private void CheckInput()
    {
        if (PlayerInput.currentControlScheme == "Gamepad")
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
