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

    /// <summary>
    /// Detects collision with the player and triggers the collection event.
    /// </summary>
    /// <param name="other">The collider that this object collides with.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCoinCollected?.Invoke();
            Deactivate();
        }
    }

    /// <summary>
    /// Rotates the coin around its axis.
    /// </summary>
    private void Rotate()
    {
        transform.Rotate(rotationAxis, 100 * Time.deltaTime);
    }

    /// <summary>
    /// Deactivates the coin game object.
    /// </summary>
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
