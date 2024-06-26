using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public Button playButton;
    public Button creditsButton;
    public Button exitButton;
    public TextMeshProUGUI playText;
    public TextMeshProUGUI creditsText;
    public TextMeshProUGUI exitText;

    public GameObject wall;
    public Transform startPoint;
    public Transform endPoint;
    public Camera mainCamera;
    public Transform cameraEnd;
    public Canvas mainCanvas;

    public int index = 1;
    public bool isJoystick = false;

    private Vector3 cameraStartPosition;
    private Quaternion cameraStartRotation;

    private bool isMovingToEnd = false;

    void Start()
    {
        playText.gameObject.SetActive(false);
        creditsText.gameObject.SetActive(false);
        exitText.gameObject.SetActive(false);

        playButton.onClick.AddListener(LoadLvl_1);
        creditsButton.onClick.AddListener(LoadCredits);
        exitButton.onClick.AddListener(Exit);

        AddEventTrigger(playButton.gameObject, EventTriggerType.PointerEnter, () => OnHoverButton(playText, true));
        AddEventTrigger(playButton.gameObject, EventTriggerType.PointerExit, () => OnHoverButton(playText, false));

        AddEventTrigger(creditsButton.gameObject, EventTriggerType.PointerEnter, () => OnHoverButton(creditsText, true));
        AddEventTrigger(creditsButton.gameObject, EventTriggerType.PointerExit, () => OnHoverButton(creditsText, false));

        AddEventTrigger(exitButton.gameObject, EventTriggerType.PointerEnter, () => OnHoverButton(exitText, true));
        AddEventTrigger(exitButton.gameObject, EventTriggerType.PointerExit, () => OnHoverButton(exitText, false));

        wall.transform.position = startPoint.position;

        cameraStartPosition = mainCamera.transform.position;
        cameraStartRotation = mainCamera.transform.rotation;
    }

    void Update()
    {
        ActiveSelected();
    }

    public void Resume(InputAction.CallbackContext callbackContext)
    {
        if (isMovingToEnd)
        {
            StartCoroutine(MoveWallAndCamera(wall.transform, endPoint.position, startPoint.position, cameraEnd.position, cameraStartPosition, cameraEnd.rotation, cameraStartRotation));
            mainCanvas.gameObject.SetActive(true);
            isMovingToEnd = false;
        }
    }

    public void ActiveSelected()
    {
        if (isJoystick)
        {
            playText.gameObject.SetActive(false);
            creditsText.gameObject.SetActive(false);
            exitText.gameObject.SetActive(false);

            switch (index)
            {
                case 1:
                    playText.gameObject.SetActive(true);
                    break;
                case 2:
                    creditsText.gameObject.SetActive(true);
                    break;
                case 3:
                    exitText.gameObject.SetActive(true);
                    break;
            }
        }
        else
        {
            index = 0;
        }
    }

    public void LoadLvl_1()
    {
        SceneManager.LoadScene("Lvl_1");
    }

    public void LoadCredits()
    {
        if (!isMovingToEnd)
        {
            StartCoroutine(MoveWallAndCamera(wall.transform, startPoint.position, endPoint.position, cameraStartPosition, cameraEnd.position, cameraStartRotation, cameraEnd.rotation));
            mainCanvas.gameObject.SetActive(false);
            isMovingToEnd = true;
            creditsText.gameObject.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

    private IEnumerator MoveWallAndCamera(Transform wallTransform, Vector3 wallStart, Vector3 wallEnd, Vector3 cameraStartPos, Vector3 cameraEndPos, Quaternion cameraStartRot, Quaternion cameraEndRot, float duration = 1.0f)
    {
        float time = 0;

        while (time < duration)
        {
            float t = time / duration;
            wallTransform.position = Vector3.Lerp(wallStart, wallEnd, t);
            mainCamera.transform.position = Vector3.Lerp(cameraStartPos, cameraEndPos, t);
            mainCamera.transform.rotation = Quaternion.Lerp(cameraStartRot, cameraEndRot, t);
            time += Time.deltaTime;
            yield return null;
        }

        wallTransform.position = wallEnd;
        mainCamera.transform.position = cameraEndPos;
        mainCamera.transform.rotation = cameraEndRot;
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
