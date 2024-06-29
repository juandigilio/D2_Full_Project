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


    private void Awake()
    {
        if (!instance)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
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

    private void Update()
    {
        if (pauseMenuUI.activeSelf != wall.activeCanvas)
        {
            pauseMenuUI.SetActive(wall.activeCanvas);
        }

        ActiveSelected();
    }

    public void Pause(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            if (!wall.isDropping && !wall.isQuiting && !gameIsPaused)
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

    public void Resume(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            Continue();
        }      
    }

    public void Continue()
    {
        if (!wall.isDropping && !wall.isQuiting)
        {
            wall.QuitWall();
            pauseMenuUI.SetActive(false);
            gameIsPaused = false;
            player.enabled = true;
        }
    }

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

    public void LoadMainMenu()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        InputManager.instance.Unsuscribe();
        CustomSceneManager.LoadSceneAsync("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

    private void OnHoverButton(TextMeshProUGUI text, bool isHovering)
    {
        text.gameObject.SetActive(isHovering);
    }

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
