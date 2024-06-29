using UnityEngine;
using System;
using System.Collections;

public class Altar : MonoBehaviour
{
    public Camera mainCamera;
    private Cameraman cameraman;
    public Player player;
    public GameObject cameraPoint;
    public Arch arch;

    public float cameraHeight = 1.5f;
    public float offsetZ = 1.0f;

    private Vector3 initialPosition;
    public float altarHeight = 3.0f;
    public float duration = 2f;
    public float animationPause = 1.5f;

    private bool isAnimating = false;
    private bool inPrayingZone = false;
    private bool hasPrayed = false;

    public static event Action OnPlayerPause;
    public static event Action OnOpenDoor;

    private void Awake()
    {
        gameObject.SetActive(false);
        cameraman = mainCamera.GetComponent<Cameraman>();

        PrayBehaviour.OnActivateQuest += IsPraying;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        
        StartCoroutine(MoveUpRoutine());
    }

    private IEnumerator MoveUpRoutine()
    {
        OnPlayerPause?.Invoke();
        player.enabled = false;
        cameraman.enabled = false;
        isAnimating = true;

        QuestCamera.SetCamera(cameraPoint, mainCamera, cameraHeight, offsetZ);

        initialPosition = transform.position;
        Vector3 targetPosition = transform.position + Vector3.up * altarHeight;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
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
 
        //OnOpenDoor?.Invoke();
    }

    private void IsPraying()
    {
        if (!hasPrayed && inPrayingZone)
        {
            OnOpenDoor?.Invoke();
            hasPrayed = true;
        }
    }

    public void PrayingZone(bool state)
    {
        inPrayingZone = state;
    }

    public bool PrayingZone()
    {
        return inPrayingZone;
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }
}
