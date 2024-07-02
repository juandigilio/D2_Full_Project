using System.Collections;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    public GameObject wall;
    public Camera mainCamera;
    public float wallDropDuration = 1.0f;
    private Vector3 wallStartPosition;
    private Vector3 wallTargetPosition;
    [SerializeField] private float forwardMagnitude = 4;
    [SerializeField] private float downMagnitude = 3.5f;
    [SerializeField] private float leftMagnitude = 2;
    [SerializeField] private bool activeCanvas = false;
    [SerializeField] private bool isQuiting = false;
    [SerializeField] private bool isDropping = false;

    private MenuSounds menuSounds;

    /// <summary>
    /// Initializes the component and deactivates the wall.
    /// </summary>
    private void Awake()
    {
        menuSounds = GetComponent<MenuSounds>();

        wall.SetActive(false);
    }

    /// <summary>
    /// Sets the target position for the wall based on camera position and displacements.
    /// </summary>
    private void SetTargetPosition()
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

    /// <summary>
    /// Activates and starts dropping the wall.
    /// </summary>
    public void DropWall()
    {
        if (wall == null)
        {
            return;
        }
        wall.SetActive(true);

        StartCoroutine(DropingWall());
    }

    /// <summary>
    /// Coroutine to handle the dropping animation of the wall.
    /// </summary>
    private IEnumerator DropingWall()
    {
        menuSounds.PlayWallSound();
        SetTargetPosition();
        isDropping = true;

        float elapsedTime = 0f;

        while (elapsedTime < wallDropDuration)
        {
            wall.transform.position = Vector3.Lerp(wallStartPosition, wallTargetPosition, elapsedTime / wallDropDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        menuSounds.StopWallSound();

        wall.transform.position = wallTargetPosition;
        activeCanvas = true;
        isDropping = false;
    }

    /// <summary>
    /// Starts the coroutine to quit the wall.
    /// </summary>
    public void QuitWall()
    {
        StartCoroutine(QuitingWall());
    }

    /// <summary>
    /// Returns whether the wall is currently dropping.
    /// </summary>
    public bool IsDropping()
    {
        return isDropping;
    }

    /// <summary>
    /// Returns whether the wall is currently quitting.
    /// </summary>
    public bool IsQuiting()
    {
        return isQuiting;
    }

    /// <summary>
    /// Returns whether the canvas is active.
    /// </summary>
    public bool ActiveCanvas()
    {
        return activeCanvas;
    }

    /// <summary>
    /// Coroutine to handle the quitting animation of the wall.
    /// </summary>
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
