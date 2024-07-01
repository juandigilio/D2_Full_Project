using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private MenuManager menuManager;

    private bool isJoystick = false;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        menuManager = GetComponent<MenuManager>();
    }

    private void Start()
    {
        playerInput.SwitchCurrentActionMap("Paused");
        if (playerInput != null)
        {


            playerInput.currentActionMap.FindAction("Up").started += Up;
            playerInput.currentActionMap.FindAction("Down").started += Down;
            playerInput.currentActionMap.FindAction("Select").started += Select;
            playerInput.currentActionMap.FindAction("Resume").started += menuManager.Resume;
        }
    }

    private void OnDestroy()
    {
        if (playerInput.currentActionMap == null)
        {
            return;
        }
        playerInput.SwitchCurrentActionMap("Paused");

        playerInput.currentActionMap.FindAction("Up").started -= Up;
        playerInput.currentActionMap.FindAction("Down").started -= Down;
        playerInput.currentActionMap.FindAction("Select").started -= Select;
        playerInput.currentActionMap.FindAction("Resume").started -= menuManager.Resume;
    }

    private void Update()
    {
        CheckInput();
        ActiveSelected();
    }

    public void Up(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            menuManager.SetIndex(menuManager.GetIndex() - 1);

            if (menuManager.GetIndex() < 1)
            {
                menuManager.SetIndex(1);
            }
        }
    }

    public void Down(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            menuManager.SetIndex(menuManager.GetIndex() + 1);

            if (menuManager.GetIndex() > 3)
            {
                menuManager.SetIndex(3);
            }
        }
    }

    public void ActiveSelected()
    {
        menuManager.TurnOffButtos();

        switch (menuManager.GetIndex())
        {
            case 1:
                menuManager.GetPlayText().gameObject.SetActive(true);
                break;
            case 2:
                menuManager.GetCreditsText().gameObject.SetActive(true);
                break;
            case 3:
                menuManager.GetExitText().gameObject.SetActive(true);
                break;
        }
    }

    public void Select(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            switch (menuManager.GetIndex())
            {
                case 1:
                    {
                        menuManager.LoadTutorial();
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

    private void CheckInput()
    {
        if (isJoystick && playerInput.currentControlScheme != "Gamepad")
        {
            menuManager.TurnOffButtos();
        }

        if (playerInput.currentControlScheme == "Gamepad")
        {
            isJoystick = true;
        }
        else
        {
            isJoystick = false;
        }
    }
}
