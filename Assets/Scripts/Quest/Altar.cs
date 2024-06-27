using UnityEngine;
using System;
using System.Collections;

public class Altar : MonoBehaviour
{
    public Camera mainCamera;
    private Cameraman cameraman;
    public Player player;
    public GameObject start;
    public GameObject end;
    public GameObject cameraPoint;

    public float cameraHeight = 1.5f;
    public float offsetZ = 1.0f;

    public float duration = 2f;
    public float animationPause = 1.5f;

    private bool isAnimating = false;

    private Vector3 initialPosition;

    public static event Action OnPlayerPause;

    void Start()
    {
        initialPosition = start.transform.position;
        Debug.Log("initialPosition: " + initialPosition); 

        cameraman = mainCamera.GetComponent<Cameraman>();

        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        
        StartCoroutine(MoveUpRoutine());
    }

    public void SetCamera()
    {
        Vector3 newPos = cameraPoint.transform.position;
        newPos.y += cameraHeight;
        newPos.z -= offsetZ;

        mainCamera.transform.position = newPos;

        mainCamera.transform.LookAt(cameraPoint.transform.position);
    }

    private IEnumerator MoveUpRoutine()
    {
        OnPlayerPause?.Invoke();
        player.enabled = false;
        cameraman.enabled = false;

        SetCamera();

        Vector3 targetPosition = end.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Debug.Log("initialPosition: " + initialPosition); 

            //Debug.Log("Target Position: " + targetPosition);

            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        elapsedTime = 0f;

        while (elapsedTime < animationPause)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.enabled = true;
        cameraman.enabled = true;
        isAnimating = false;
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }
}
