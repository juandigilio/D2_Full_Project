using UnityEngine;
using System;

public class Coin : MonoBehaviour, ICollectable
{
    private Vector3 rotationAxis = new Vector3(0, 0, 1);

    public static event Action OnCoinCollected;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        Rotate();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colliding");
        if (other.CompareTag("Player"))
        {
            OnCoinCollected?.Invoke();

            Deactivate();
        }
    }

    private void Rotate()
    {
        transform.Rotate(rotationAxis, 100 * Time.deltaTime);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
