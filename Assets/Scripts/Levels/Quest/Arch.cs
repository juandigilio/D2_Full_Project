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

    public float cameraHeight = 1.5f;
    public float offsetZ = 1.0f;

    private Vector3 initialPosition;
    public float doorHeight = 3.0f;
    public float duration = 2f;
    public float animationPause = 1.5f;

    private bool isAnimating;

    public static event Action OnPlayerPause;

    private void Awake()
    {
        grate = GameObject.Find("Grate");
        if (grate == null)
        {
            Debug.Log("No grate found");
        }

        cameraman = mainCamera.GetComponent<Cameraman>();
        doorSOund = GetComponent<DoorSound>();

        Altar.OnOpenDoor += Open;
    }

    private void Open()
    {
        doorSOund.PlayDoorSound();
        StartCoroutine(MoveUpRoutine());
    }

    private IEnumerator MoveUpRoutine()
    {
        OnPlayerPause?.Invoke();
        player.enabled = false;
        cameraman.enabled = false;

        isAnimating = true;

        QuestCamera.SetCamera(cameraPoint, mainCamera, cameraHeight, offsetZ);

        initialPosition = grate.transform.position;
        Vector3 targetPosition = grate.transform.position + Vector3.up * doorHeight;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Debug.Log("initialPosition: " + initialPosition);

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
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }
}
