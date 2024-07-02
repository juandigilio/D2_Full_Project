using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    private PauseManager instance;

    public bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public Player player;
    public WallBehaviour wall;

    public Button continueButton;
    public Button menuButton;
    public Button exitButton;
    public TextMeshProUGUI continueText;
    public TextMeshProUGUI menuText;
    public TextMeshProUGUI exitText;

    private int index = 1;
    public bool isJoystick = false;

    /// <summary>
    /// Initializes the pause manager and sets up button listeners.
    /// </summary>
    private void Awake()
    {
        if (!instance)
        {
            //DontDestroyOnLoad(gameObject);
            //instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }

        pauseMenuUI.SetActive(false);

        continueButton.onClick.AddListener(Continue);
        menuButton.onClick.AddListener(LoadMainMenu);
        exitButton.onClick.AddListener(Exit);

        continueText.gameObject.SetActive(false);
        menuText.gameObject.SetActive(false);
        exitText.gameObject.SetActive(false);

        AddEventTrigger(continueButton.gameObject, EventTriggerType.PointerEnter, () => OnHoverButton(continueText, true));
        AddEventTrigger(continueButton.gameObject, EventTriggerType.PointerExit, () => OnHoverButton(continueText, false));

        AddEventTrigger(menuButton.gameObject, EventTriggerType.PointerEnter, () => OnHoverButton(menuText, true));
        AddEventTrigger(menuButton.gameObject, EventTriggerType.PointerExit, () => OnHoverButton(menuText, false));

        AddEventTrigger(exitButton.gameObject, EventTriggerType.PointerEnter, () => OnHoverButton(exitText, true));
        AddEventTrigger(exitButton.gameObject, EventTriggerType.PointerExit, () => OnHoverButton(exitText, false));
    }

    /// <summary>
    /// Updates the pause menu visibility and active selection state.
    /// </summary>
    private void Update()
    {
        if (pauseMenuUI.activeSelf != wall.ActiveCanvas())
        {
            pauseMenuUI.SetActive(wall.ActiveCanvas());
        }

        ActiveSelected();
    }

    /// <summary>
    /// Pauses the game and shows the pause menu.
    /// </summary>
    public void Pause(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            if (!wall.IsDropping() && !wall.IsQuiting() && !gameIsPaused)
            {
                Time.timeScale = 0f;
                gameIsPaused = true;
                if (player == null)
                {
                    player = FindAnyObjectByType<Player>();
                }
                player.enabled = false;

                wall.DropWall();
            }
        }
    }

    /// <summary>
    /// Resumes the game from the pause menu.
    /// </summary>
    public void Resume(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            Continue();
        }
    }

    /// <summary>
    /// Continues the game and hides the pause menu.
    /// </summary>
    public void Continue()
    {
        if (!wall.IsDropping() && !wall.IsQuiting())
        {
            wall.QuitWall();
            pauseMenuUI.SetActive(false);
            gameIsPaused = false;
            player.enabled = true;
        }
    }

    /// <summary>
    /// Moves the selection up in the pause menu.
    /// </summary>
    public void Up(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            index--;

            if (index < 1)
            {
                index = 1;
            }
        }
    }

    /// <summary>
    /// Moves the selection down in the pause menu.
    /// </summary>
    public void Down(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            index++;

            if (index > 3)
            {
                index = 3;
            }
        }
    }

    /// <summary>
    /// Activates the selected option text color in the pause menu.
    /// </summary>
    public void ActiveSelected()
    {
        if (isJoystick)
        {
            continueText.gameObject.SetActive(false);
            menuText.gameObject.SetActive(false);
            exitText.gameObject.SetActive(false);

            switch (index)
            {
                case 1:
                    {
                        continueText.gameObject.SetActive(true);
                        break;
                    }
                case 2:
                    {
                        menuText.gameObject.SetActive(true);
                        break;
                    }
                case 3:
                    {
                        exitText.gameObject.SetActive(true);
                        break;
                    }
            }
        }
        else
        {
            index = 0;
        }
    }

    /// <summary>
    /// Selects the current option in the pause menu.
    /// </summary>
    public void Select(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            switch (index)
            {
                case 1:
                    {
                        Continue();
                        break;
                    }
                case 2:
                    {
                        LoadMainMenu();
                        break;
                    }
                case 3:
                    {
                        Exit();
                        break;
                    }
            }
        }
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        InputManager.instance.Unsuscribe();
        CustomSceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Exits the game application.
    /// </summary>
    public void Exit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

    /// <summary>
    /// Toggles the hover state for the specified button text.
    /// </summary>
    private void OnHoverButton(TextMeshProUGUI text, bool isHovering)
    {
        text.gameObject.SetActive(isHovering);
    }

    /// <summary>
    /// Adds an event trigger to a UI object.
    /// </summary>
    private void AddEventTrigger(GameObject obj, EventTriggerType type, System.Action action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };

        entry.callback.AddListener((eventData) => { action(); });
        trigger.triggers.Add(entry);
    }
}
