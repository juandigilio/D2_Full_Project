using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private MenuManager menuManager;
    private MenuSounds menuSounds;

    private bool isJoystick = false;
    private int lastActive = 0;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        menuManager = GetComponent<MenuManager>();
        menuSounds = GetComponent<MenuSounds>();
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

        if (menuManager.GetIndex() == 0)
        {
            lastActive = menuManager.GetIndex();
        }

        switch (menuManager.GetIndex())
        {
            case 1:
                {
                    menuManager.GetPlayText().gameObject.SetActive(true);

                    if (lastActive != menuManager.GetIndex())
                    {
                        menuSounds.PlaySelectSound();
                        lastActive = 1;
                    }         
                    break;
                }
            case 2:
                {

                    menuManager.GetCreditsText().gameObject.SetActive(true);

                    if (lastActive != menuManager.GetIndex())
                    {
                        menuSounds.PlaySelectSound();
                        lastActive = 2;
                    }
                    break;
                }
            case 3:
                {
                    menuManager.GetExitText().gameObject.SetActive(true);

                    if (lastActive != menuManager.GetIndex())
                    {
                        menuSounds.PlaySelectSound();
                        lastActive = 3;
                    }
                    break;
                }
        }
    }

    public void Select(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            menuSounds.PlayEnterSound();

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
