using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Player Player;
    public GameObject coinsPull;
    public Altar altar;
    public GameObject door;
    private bool isAnimating = false;

    private int totalCoins;
    private int collectedCoins = 0;
    private bool allCoinsCollected = false;


    private void Start()
    {
        Coin.OnCoinCollected += CollectCoin;
    }

    private void Awake()
    {
        coinsPull = GameObject.Find("CoinsPull");
        //altar = GetComponent<Altar>();
        door = GameObject.Find("MainDoor");

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

    public void CollectCoin()
    {
        collectedCoins++;
        //Debug.Log("Collected coins: " + collectedCoins + "/" + totalCoins);
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

}
