using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private JumpBehaviour jumpBehaviour;
    [SerializeField] private PrayBehaviour prayBehaviour;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private LevelManager level;
    [SerializeField] private float mouseSensivity = 0.1f;
    public float cameraRotation = 0;

    [SerializeField] private string jumpAction = "Jump";
    [SerializeField] private string prayAction = "Pray";
    [SerializeField] private string pauseAction = "Pause";
    [SerializeField] private string moveAction = "Move";
    [SerializeField] private string resumeAction = "Resume";
    [SerializeField] private string upAction = "Up";
    [SerializeField] private string downAction = "Down";
    [SerializeField] private string selectAction = "Select";
    [SerializeField] private string pausedAction = "Paused";
    [SerializeField] private string playerAction = "Player";

    public static InputManager instance;
    public PlayerInput PlayerInput { get; private set; }

    private void Awake()
    {
        instance = this;

        PlayerInput = GetComponent<PlayerInput>();
    }

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

            PlayerInput.SwitchCurrentActionMap(pausedAction);

            PlayerInput.currentActionMap.FindAction(resumeAction).started += pauseManager.Resume;
            PlayerInput.currentActionMap.FindAction(upAction).started += pauseManager.Up;
            PlayerInput.currentActionMap.FindAction(downAction).started += pauseManager.Down;
            PlayerInput.currentActionMap.FindAction(selectAction).started += pauseManager.Select;

            PlayerInput.SwitchCurrentActionMap(playerAction);
        }
    }

    private void Update()
    {
        if (pauseManager.gameIsPaused && PlayerInput.currentActionMap.name != pausedAction)
        {
            PlayerInput.SwitchCurrentActionMap(pausedAction);
        }

        if (!pauseManager.gameIsPaused && PlayerInput.currentActionMap.name != playerAction)
        {
            PlayerInput.SwitchCurrentActionMap(playerAction);
        }

        CheckInput();
        MoveCamera();
    }

    public void Unsuscribe()
    {
        PlayerInput.SwitchCurrentActionMap(playerAction);
        PlayerInput.currentActionMap.FindAction(jumpAction).started -= jumpBehaviour.Jump;
        PlayerInput.currentActionMap.FindAction(prayAction).started -= prayBehaviour.Pray;
        PlayerInput.currentActionMap.FindAction(pausedAction).started -= pauseManager.Pause;
        PlayerInput.currentActionMap.FindAction(moveAction).started -= Move;
        PlayerInput.currentActionMap.FindAction(moveAction).performed -= Move;
        PlayerInput.currentActionMap.FindAction(moveAction).canceled -= Move;

        PlayerInput.SwitchCurrentActionMap(pausedAction);
        PlayerInput.currentActionMap.FindAction(resumeAction).started -= pauseManager.Resume;
        PlayerInput.currentActionMap.FindAction(upAction).started -= pauseManager.Up;
        PlayerInput.currentActionMap.FindAction(downAction).started -= pauseManager.Down;
        PlayerInput.currentActionMap.FindAction(selectAction).started -= pauseManager.Select;
        PlayerInput.SwitchCurrentActionMap(playerAction);
    }

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

    public void MoveCamera()
    {
        if (!pauseManager.gameIsPaused)
        {
            cameraRotation += Mouse.current.delta.ReadValue().x * mouseSensivity;
        }
    }

    public void CheckInput()
    {
        if (PlayerInput.currentControlScheme == "Gamepad")
        {
            pauseManager.isJoystick = true;
        }
        else
        {
            pauseManager.isJoystick = false;
        }
    }
}
