using System;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public static Action OnLevelFinished;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player trigger Exit Zone!!");
            OnLevelFinished?.Invoke();
        }
    }
}
