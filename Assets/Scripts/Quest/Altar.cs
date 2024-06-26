using UnityEngine;
using System.Collections;

public class Altar : MonoBehaviour
{
    public Camera mainCamera;
    private float cameraDistance;
    private float cameraHeight = 3.5f;
    private float offsetZ = 3.0f;

    public float distance = 5f;
    public float duration = 2f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    public void MoveUp()
    {
        SetCamera();
        StartCoroutine(MoveUpRoutine());
    }

    public void SetCamera()
    {
        Vector3 newPos = transform.position;
        newPos.y += cameraHeight;
        newPos.z -= offsetZ;

        mainCamera.transform.position = newPos;

        mainCamera.transform.LookAt(transform);
    }

    private IEnumerator MoveUpRoutine()
    {
        Time.timeScale = 0f;

        Vector3 targetPosition = initialPosition + Vector3.up * distance;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        Time.timeScale = 1f;
    }
}
