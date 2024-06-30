using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player Player;
    [SerializeField] private GameObject coinsPull;
    [SerializeField] private Altar altar;
    [SerializeField] private Arch door;
    [SerializeField] private Transform deathZone;

    private static int index = 0;

    private bool isAnimating = false;

    private int totalCoins;
    private int collectedCoins = 0;
    private bool allCoinsCollected = false;


    private void Start()
    {
        Coin.OnCoinCollected += CollectCoin;
        ExitZone.OnLevelFinished += LoadNextLevel;
    }

    private void Awake()
    {
        coinsPull = GameObject.Find("CoinsPull");
        GetCoins();
    }

    private void Update()
    {
        CheckCoins();
    }

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

    private void CollectCoin()
    {
        collectedCoins++;
    }

    private void CheckCoins()
    {
        if (collectedCoins == totalCoins && !allCoinsCollected)
        {
            allCoinsCollected = true;
            altar.Activate();
        }
    }

    public bool IsAnimating()
    {
        if (altar.IsAnimating())
        {
            return isAnimating;
        }
        else
        {
            return isAnimating;
        }
    }

    private void LoadNextLevel()
    {
        CustomSceneManager.UnLoadSceneAsync(SceneActionDictionary.GetScenesList()[index].sceneName);
        index++;

        if (index > SceneActionDictionary.GetScenesList().Count)
        {
            index = 0;
        }

        CustomSceneManager.LoadSceneAsync(SceneActionDictionary.GetScenesList()[index].sceneName);
    }
}
