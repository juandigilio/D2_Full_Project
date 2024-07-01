using UnityEngine;
using System;
using System.Collections;

public class Altar : MonoBehaviour
{
    private Cameraman cameraman;
    private AltarSound altarSound;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Player player;
    [SerializeField] private GameObject cameraPoint;
    [SerializeField] private Arch arch;

    [SerializeField] private float cameraHeight = 1.5f;
    [SerializeField] private float offsetZ = 1.0f;

    private Vector3 initialPosition;
    [SerializeField] private float altarHeight = 3.0f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float animationPause = 1.5f;

    private bool isAnimating = false;
    private bool inPrayingZone = false;
    private bool hasPrayed = false;

    public static event Action OnPlayerPause;
    public static event Action OnOpenDoor;

    private void Awake()
    {
        gameObject.SetActive(false);
        cameraman = mainCamera.GetComponent<Cameraman>();
        altarSound = GetComponent<AltarSound>();

        PrayBehaviour.OnActivateQuest += IsPraying;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        altarSound.PlayAltarSound();
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
