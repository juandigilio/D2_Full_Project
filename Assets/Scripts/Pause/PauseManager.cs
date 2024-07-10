using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    //private PauseManager instance;

    [SerializeField] private bool gameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Player player;
    [SerializeField] private WallBehaviour wall;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private TextMeshProUGUI menuText;
    [SerializeField] private TextMeshProUGUI exitText;
    [SerializeField] private Slider mouseSlider;
    [SerializeField] private Slider padSlider;

    private int index = 1;
    [SerializeField] private bool isJoystick = false;

    /// <summary>
    /// Initializes the pause manager and sets up button listeners.
    /// </summary>
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PauseManager");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
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

        //mouseSlider.value = 0.1f;
        //padSlider.value = 3.0f;

        mouseSlider.onValueChanged.AddListener(SetMouseSensitivity);
        padSlider.onValueChanged.AddListener(SetPadSensitivity);
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveListener(Continue);
        menuButton.onClick.RemoveListener(LoadMainMenu);
        exitButton.onClick.RemoveListener(Exit);
    }

    private void OnDestroy()
    {
        continueButton.onClick.RemoveListener(Continue);
        menuButton.onClick.RemoveListener(LoadMainMenu);
        exitButton.onClick.RemoveListener(Exit);
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
        ActiveCurrentSlider();
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
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// Continues the game and hides the pause menu.
    /// </summary>
    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
    /// Activates the current slider based on input type.
    /// </summary>
    public void ActiveCurrentSlider()
    {
        if (isJoystick)
        {
            if (PlayerConfig.GetPadSensitivity() == 0)
            {
                PlayerConfig.SetPadSensitivity(1);
            }
            mouseSlider.gameObject.SetActive(false);
            padSlider.gameObject.SetActive(true);
        }
        else
        {
            if (PlayerConfig.GetMouseSensitivity() == 0)
            {
                PlayerConfig.SetMouseSensitivity(0.1f);
            }
            mouseSlider.gameObject.SetActive(true);
            padSlider.gameObject.SetActive(false);
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
        //InputManager.instance.Unsuscribe();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        GameObject input = GameObject.FindGameObjectWithTag("InputManager");
        GameObject pause = GameObject.FindGameObjectWithTag("PauseManager");
        GameObject eventSys = GameObject.FindGameObjectWithTag("EventSystem");

        if (player)
        {
            Destroy(player);
        }

        if (camera)
        {
            Destroy(camera);
        }

        if (input)
        {
            Destroy(input);
        }
        else
        {
            Debug.Log("no input to delete!");
        }

        if (eventSys)
        {
            Destroy(eventSys);
        }

        if (pause)
        {
            Destroy(pause);
        }
        else
        {
            Debug.Log("no pause to delete!");
        }

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

    public bool GameIsPaused()
    {
        return gameIsPaused;
    }

    public void IsJoystick(bool set)
    {
        isJoystick = set;
    }

    /// <summary>
    /// Sets the mouse sensitivity.
    /// </summary>
    /// <param name="value">The new sensitivity value.</param>
    public void SetMouseSensitivity(float value)
    {
        PlayerConfig.SetMouseSensitivity(value);
    }

    /// <summary>
    /// Sets the pad sensitivity.
    /// </summary>
    /// <param name="value">The new sensitivity value.</param>
    public void SetPadSensitivity(float value)
    {
        PlayerConfig.SetPadSensitivity(value);
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
