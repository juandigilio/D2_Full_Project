using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

        wall.transform.position = startPoint.position;

        cameraStartPosition = mainCamera.transform.position;
        cameraStartRotation = mainCamera.transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isMovingToEnd)
        {
            StartCoroutine(MoveWallAndCamera(wall.transform, endPoint.position, startPoint.position, cameraEnd.position, cameraStartPosition, cameraEnd.rotation, cameraStartRotation));
            mainCanvas.gameObject.SetActive(true);
            isMovingToEnd = false;
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
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ShowPlayText()
    {
        playText.gameObject.SetActive(true);
    }

    public void HidePlayText()
    {
        playText.gameObject.SetActive(false);
    }

    public void ShowCreditsText()
    {
        creditsText.gameObject.SetActive(true);
    }

    public void HideCreditsText()
    {
        creditsText.gameObject.SetActive(false);
    }

    public void ShowExitText()
    {
        exitText.gameObject.SetActive(true);
    }

    public void HideExitText()
    {
        exitText.gameObject.SetActive(false);
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
}