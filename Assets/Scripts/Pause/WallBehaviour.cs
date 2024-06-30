using System.Collections;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    public GameObject wall;
    public Camera mainCamera;
    public float wallDropDuration = 1.0f;
    private Vector3 wallStartPosition;
    private Vector3 wallTargetPosition;
    public float forwardMagnitude = 4;
    public float downMagnitude = 3.5f;
    public float leftMagnitude = 2;
    public bool activeCanvas = false;
    public bool isQuiting = false;
    public bool isDropping = false;

    private void Awake()
    {
        wall.SetActive(false);
    }

    void SetTargetPosition()
    {
        wallTargetPosition = mainCamera.transform.position;

        Vector3 forwardDisplacement = mainCamera.transform.forward;
        Vector3 downDisplacement = -mainCamera.transform.up;
        Vector3 leftDisplacement = -mainCamera.transform.right;

        wallTargetPosition += forwardDisplacement * forwardMagnitude;
        wallTargetPosition += downDisplacement * downMagnitude;
        wallTargetPosition += leftDisplacement * leftMagnitude;

        wallStartPosition = wallTargetPosition + (wall.transform.up * 7);
        wall.transform.position = wallStartPosition;
    }

    public void DropWall()
    {
        if (wall == null)
        {
            return;
        }
        wall.SetActive(true);

        StartCoroutine(DropingWall());
    }

    private IEnumerator DropingWall()
    {
        SetTargetPosition();
        isDropping = true;

        float elapsedTime = 0f;

        while (elapsedTime < wallDropDuration)
        {
            wall.transform.position = Vector3.Lerp(wallStartPosition, wallTargetPosition, elapsedTime / wallDropDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        wall.transform.position = wallTargetPosition;
        activeCanvas = true;
        isDropping = false;        
    }

    public void QuitWall()
    {
        StartCoroutine(QuitingWall());
    }

    private IEnumerator QuitingWall()
    {
        activeCanvas = false;
        isQuiting = true;

        float elapsedTime = 0f;

        while (elapsedTime < wallDropDuration)
        {
            SetTargetPosition();
            wall.transform.position = Vector3.Lerp(wallTargetPosition, wallStartPosition, elapsedTime / wallDropDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        wall.transform.position = wallStartPosition;
        isQuiting = false;
        wall.SetActive(false);
        Time.timeScale = 1f;
    }
}
