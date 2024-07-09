using UnityEngine;

public class Cameraman : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private InputManager inputManager;
    private float distance;
    private float height = 3.5f;
    private float offsetZ = 3.0f;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MainCamera");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }

        InitCamera();

        //GameObject input = GameObject.FindGameObjectWithTag("InputManager");
        //inputManager = input.GetComponent<InputManager>();
    }

    void LateUpdate()
    {
        UpdatePosition();
    }

    /// <summary>
    /// Initializes the camera position relative to the player.
    /// </summary>
    private void InitCamera()
    {
        mainCamera = Camera.main.GetComponent<Camera>();

        Vector3 newPos = playerTransform.position;
        newPos.y += height;
        newPos.z -= offsetZ;

        mainCamera.transform.position = newPos;

        distance = Vector3.Distance(playerTransform.position, mainCamera.transform.position);
    }

    /// <summary>
    /// Updates the camera position based on player movement and rotation input.
    /// </summary>
    private void UpdatePosition()
    {
        Vector3 offset = new Vector3(0, height, -distance);
        Quaternion newRotation = Quaternion.Euler(0, inputManager.GetCameraRotation(), 0);
        Vector3 rotatedOffset = newRotation * offset;
        Vector3 newPos = playerTransform.position + rotatedOffset;

        mainCamera.transform.position = newPos;

        mainCamera.transform.LookAt(playerTransform);
    }
}
