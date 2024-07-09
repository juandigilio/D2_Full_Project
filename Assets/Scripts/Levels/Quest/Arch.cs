using System.Collections;
using System;
using UnityEngine;

public class Arch : MonoBehaviour
{
    private Cameraman cameraman;
    private GameObject grate;
    private DoorSound doorSOund;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Player player;
    [SerializeField] private GameObject cameraPoint;

    public float cameraHeight = -1.0f;
    public float offsetZ = -5.0f;

    private Vector3 initialPosition;
    [SerializeField] private float doorHeight = 3.0f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float animationPause = 3.0f;

    private bool isAnimating;
    private bool isOpen = false;

    public static event Action OnDoorAnimationStarted;
    public static event Action OnDoorAnimationFinished;

    private void Awake()
    {
        grate = GameObject.Find("Grate");
        if (grate == null)
        {
            Debug.Log("No grate found");
        }

        GameObject obj = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera = obj.GetComponent<Camera>();

        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        player = tempPlayer.GetComponent<Player>();

        cameraman = mainCamera.GetComponent<Cameraman>();
        doorSOund = GetComponent<DoorSound>();

        Altar.OnOpenDoor += Open;
        CheatsManager.OnOpenDoor += Open;
    }

    private void OnDisable()
    {
        Altar.OnOpenDoor -= Open;
        CheatsManager.OnOpenDoor -= Open;
    }

    private void OnDestroy()
    {
        Altar.OnOpenDoor -= Open;
        CheatsManager.OnOpenDoor -= Open;
    }

    private void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            doorSOund.PlayDoorSound();
            StartCoroutine(MoveUpRoutine());
        }
        
    }

    private IEnumerator MoveUpRoutine()
    {
        OnDoorAnimationStarted?.Invoke();
        player.enabled = false;
        cameraman.enabled = false;

        isAnimating = true;

        QuestCamera.SetCamera(cameraPoint, mainCamera, cameraHeight, offsetZ);

        initialPosition = grate.transform.position;
        Vector3 targetPosition = grate.transform.position + Vector3.up * doorHeight;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            //Debug.Log("initialPosition: " + initialPosition);

            grate.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        grate.transform.position = targetPosition;

        elapsedTime = 0f;

        while (elapsedTime < animationPause)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.enabled = true;
        cameraman.enabled = true;
        isAnimating = false;

        OnDoorAnimationFinished?.Invoke();
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }
}
