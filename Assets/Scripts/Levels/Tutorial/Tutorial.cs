using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject coinsPull;
    public GameObject altar;
    public GameObject door;

    private int totalCoins;
    private int collectedCoins = 0;


    private void Start()
    {
        Coin.OnCoinCollected += CollectCoin;
    }

    private void Awake()
    {
        coinsPull = GameObject.Find("CoinsPull");
        altar = GameObject.Find("Altar");
        door = GameObject.Find("MainDoor");

        GetCoins();
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
        Debug.Log("Collected coins: " + collectedCoins + "/" + totalCoins);
    }

    private void CheckStatus()
    {
        if (collectedCoins ==  totalCoins)
        {

        }
    }
}
