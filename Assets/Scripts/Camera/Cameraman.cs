using UnityEngine;

public class Cameraman : MonoBehaviour
{
    public Camera mainCamera;
    public Transform playerTransform;
    [SerializeField] private InputManager inputManager;
    private float distance;
    private float height = 3.5f;
    private float offsetZ = 3.0f;

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        InitCamera();
        inputManager = InputManager.instance;
    }

    void LateUpdate()
    {
        UpdatePosition();
    }

    private void InitCamera()
    {
        mainCamera = Camera.main.GetComponent<Camera>();

        Vector3 newPos = playerTransform.position;
        newPos.y += height;
        newPos.z -= offsetZ;

        mainCamera.transform.position = newPos;

        distance = Vector3.Distance(playerTransform.position, mainCamera.transform.position);
    }

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
