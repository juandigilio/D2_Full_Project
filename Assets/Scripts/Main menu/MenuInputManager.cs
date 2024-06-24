using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    private MenuManager menuManager;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        menuManager = GetComponent<MenuManager>();
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("Paused");

            playerInput.currentActionMap.FindAction("Up").started += Up;
            playerInput.currentActionMap.FindAction("Down").started += Down;
            playerInput.currentActionMap.FindAction("Select").started += Select;
            playerInput.currentActionMap.FindAction("Resume").started += menuManager.Resume;
        }
    }

    private void OnDisable()
    {
        //playerInput.SwitchCurrentActionMap("Paused");

        playerInput.currentActionMap.FindAction("Up").started -= Up;
        playerInput.currentActionMap.FindAction("Down").started -= Down;
        playerInput.currentActionMap.FindAction("Select").started -= Select;
        playerInput.currentActionMap.FindAction("Resume").started -= menuManager.Resume;
    }

    private void Update()
    {
        CheckInput();
    }

    public void Up(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            menuManager.index--;

            if (menuManager.index < 1)
            {
                menuManager.index = 1;
            }
        }
    }

    public void Down(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            menuManager.index++;

            if (menuManager.index > 3)
            {
                menuManager.index = 3;
            }
        }
    }

    public void Select(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            switch (menuManager.index)
            {
                case 1:
                    {
                        menuManager.LoadLvl_1();
                        break;
                    }
                case 2:
                    {
                        menuManager.LoadCredits();
                        break;
                    }
                case 3:
                    {
                        menuManager.Exit();
                        break;
                    }
            }
        }
    }

    public void CheckInput()
    {
        if (playerInput.currentControlScheme == "Gamepad")
        {
            menuManager.isJoystick = true;
        }
        else
        {
            menuManager.isJoystick = false;
        }
    }
}
