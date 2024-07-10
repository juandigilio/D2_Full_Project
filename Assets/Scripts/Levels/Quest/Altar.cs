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

    [SerializeField] private float cameraHeight = -1.0f;
    [SerializeField] private float offsetZ = 3f;

    private Vector3 initialPosition;
    [SerializeField] private float altarHeight = 3.0f;
    [SerializeField] private float duration = 4f;
    [SerializeField] private float animationPause = 1.0f;

    private bool isAnimating = false;
    private bool inPrayingZone = false;
    private bool hasPrayed = false;
    private bool nextLevelLoaded = false;

    public static event Action OnAltarAnimationStarted;
    public static event Action OnAltarAnimationFinished;
    public static event Action OnOpenDoor;

    /// <summary>
    /// Initializes the Altar and subscribes to the OnActivateQuest event.
    /// </summary>
    private void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera = obj.GetComponent<Camera>();

        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        player = tempPlayer.GetComponent<Player>();

        gameObject.SetActive(false);
        cameraman = mainCamera.GetComponent<Cameraman>();
        altarSound = GetComponent<AltarSound>();

        PrayBehaviour.OnActivateQuest += PrayFinished;
        PrayBehaviour.OnAnimationPraying += IsPraying;
        CheatsManager.OnOpenDoor += CheatDoor;
    }

    /// <summary>
    /// Unsubscribes from the OnActivateQuest event when the Altar is disabled.
    /// </summary>
    private void OnDisable()
    {
        PrayBehaviour.OnActivateQuest -= PrayFinished;
        PrayBehaviour.OnAnimationPraying -= IsPraying;
        CheatsManager.OnOpenDoor -= CheatDoor;
    }

    /// <summary>
    /// Activates the Altar and starts the MoveUpRoutine coroutine.
    /// </summary>
    public void Activate()
    {
        gameObject.SetActive(true);
        altarSound.PlayAltarSound();
        StartCoroutine(MoveUpRoutine());
    }

    /// <summary>
    /// Moves the Altar up over a duration and pauses before re-enabling player and cameraman.
    /// </summary>
    private IEnumerator MoveUpRoutine()
    {
        OnAltarAnimationStarted?.Invoke();
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

        OnAltarAnimationFinished?.Invoke();
    }

    private void IsPraying()
    {
        if (!nextLevelLoaded && inPrayingZone)
        {
            nextLevelLoaded = true;
            CustomSceneManager.LoadNextSceneAsync();
        }
    }

    /// <summary>
    /// Checks if the player is praying and opens the door if conditions are met.
    /// </summary>
    private void PrayFinished()
    {
        if (!hasPrayed && inPrayingZone)
        {
            hasPrayed = true;
            OnOpenDoor?.Invoke();
        }
    }

    private void CheatDoor()
    {
        CustomSceneManager.LoadNextSceneAsync();
        nextLevelLoaded = true;
        hasPrayed = true;
        OnOpenDoor?.Invoke();
    }

    /// <summary>
    /// Sets the state of the praying zone.
    /// </summary>
    public void PrayingZone(bool state)
    {
        inPrayingZone = state;
    }

    /// <summary>
    /// Returns whether the player is in the praying zone.
    /// </summary>
    public bool PrayingZone()
    {
        return inPrayingZone;
    }

    /// <summary>
    /// Returns whether the Altar is currently animating.
    /// </summary>
    public bool IsAnimating()
    {
        return isAnimating;
    }

    public bool HasPrayed()
    {
        return hasPrayed;
    }
}
