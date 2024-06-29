using System;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public static Action OnLevelFinished;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnLevelFinished?.Invoke();
        }
    }
}
