using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    private Vector3 rotationAxis = new Vector3(0, 0, 1);
    private Transform transform;


    public static event Action OnCoinCollected;


    private void Start()
    {
        transform = GetComponent<Transform>();    
    }

    private void Update()
    {
        Rotate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnCoinCollected?.Invoke();
            
            Destroy(gameObject);     
        }
    }

    private void Rotate()
    {
        transform.Rotate(rotationAxis, 100 * Time.deltaTime);
    }
}
