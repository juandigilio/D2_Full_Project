using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI playText;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI exitText;

    [SerializeField] private GameObject wall;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraEnd;
    [SerializeField] private Canvas mainCanvas;

    private Vector3 cameraStartPosition;
    private Quaternion cameraStartRotation;

    private bool isMovingToEnd = false;

    private int index = 0;

    void Start()
    {
        TurnOffButtos();

        playButton.onClick.AddListener(LoadTutorial);
        creditsButton.onClick.AddListener(LoadCredits);
        exitButton.onClick.AddListener(Exit);

        AddEventTrigger(playButton.gameObject, EventTriggerType.PointerEnter, () => OnHoverButton(1));
        AddEventTrigger(playButton.gameObject, EventTriggerType.PointerExit, () => OnHoverButton(0));

        AddEventTrigger(creditsButton.gameObject, EventTriggerType.PointerEnter, () => OnHoverButton(2));
        AddEventTrigger(creditsButton.gameObject, EventTriggerType.PointerExit, () => OnHoverButton(0));

        AddEventTrigger(exitButton.gameObject, EventTriggerType.PointerEnter, () => OnHoverButton(3));
        AddEventTrigger(exitButton.gameObject, EventTriggerType.PointerExit, () => OnHoverButton(0));

        wall.transform.position = startPoint.position;

        cameraStartPosition = mainCamera.transform.position;
        cameraStartRotation = mainCamera.transform.rotation;
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

    public void LoadTutorial()
    {
        CustomSceneManager.LoadNextSceneAsync();
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

    private void OnHoverButton(int hoverIndex)
    {
        index = hoverIndex;
        //Debug.Log($"Hover Index: {hoverIndex}");
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

    public TextMeshProUGUI GetPlayText()
    {
        return playText;
    }

    public TextMeshProUGUI GetCreditsText()
    {
        return creditsText;
    }

    public TextMeshProUGUI GetExitText()
    {
        return exitText;
    }

    public int GetIndex()
    {
        return index;
    }

    public void SetIndex(int newIndex)
    {
        index = newIndex;
    }

    public void TurnOffButtos()
    {
        GetPlayText().gameObject.SetActive(false);
        GetCreditsText().gameObject.SetActive(false);
        GetExitText().gameObject.SetActive(false);
    }
}
