using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject coinsPull;
    [SerializeField] private Altar altar;
    [SerializeField] private Arch door;
    [SerializeField] private Transform deathZone;
    [SerializeField] private InputManager inputManager;

    private bool isAnimating = false;

    private int totalCoins;
    private int collectedCoins = 0;
    private bool allCoinsCollected = false;

    public static Action onDeathZone;


    private void Start()
    {
        Coin.OnCoinCollected += CollectCoin;
        ExitZone.OnLevelFinished += LoadNextLevel;
        WiningZone.OnGameFinished += LoadWiningScene;
    }

    private void Awake()
    {
        coinsPull = GameObject.Find("CoinsPull");
        GetCoins();
    }

    private void Update()
    {
        CheckCoins();
        CheckDeathZone();
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= CollectCoin;
        ExitZone.OnLevelFinished -= LoadNextLevel;
        WiningZone.OnGameFinished -= LoadWiningScene;
    }

    /// <summary>
    /// Retrieves the total number of coins in the level.
    /// </summary>
    private void GetCoins()
    {
        if (coinsPull != null)
        {
            totalCoins = coinsPull.transform.childCount;
        }
        else
        {
            Debug.LogWarning("Coins pull not found");
        }
    }

    /// <summary>
    /// Handles the event of collecting a coin.
    /// </summary>
    private void CollectCoin()
    {
        collectedCoins++;
    }

    /// <summary>
    /// Checks if all coins have been collected and activates the altar if true.
    /// </summary>
    private void CheckCoins()
    {
        if (collectedCoins == totalCoins && !allCoinsCollected)
        {
            allCoinsCollected = true;
            altar.Activate();
        }
    }

    private void CheckDeathZone()
    {
        if (player.MovementBehaviour().PosY() < deathZone.position.y)
        {
            onDeathZone?.Invoke();
            Debug.Log("death zone");
        }
    }

    /// <summary>
    /// Checks if the altar is currently animating.
    /// </summary>
    public bool IsAnimating()
    {
        return altar.IsAnimating();
    }

    /// <summary>
    /// Loads the next level when the exit zone is triggered.
    /// </summary>
    private void LoadNextLevel()
    {
        inputManager.Unsuscribe();
        Destroy(coinsPull);
        Destroy(altar.gameObject);
        Destroy(door.gameObject);
        CustomSceneManager.LoadNextSceneAsync();
    }

    private void LoadWiningScene()
    {
        inputManager.Unsuscribe();
        Destroy(coinsPull);
        Destroy(altar.gameObject);
        Destroy(door.gameObject);
        CustomSceneManager.LoadWiningScene();
    }
}
