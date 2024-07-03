using System;
using UnityEngine;

public class WiningZone : MonoBehaviour
{
    public static Action OnGameFinished;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnGameFinished?.Invoke();
        }
    }
}
