using System;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public static Action OnLevelFinished;
    private bool hasTriggered = false;

    /// <summary>
    /// Unload last scene when triggering
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasTriggered)
            {
                Debug.Log("Player trigger Exit Zone!!");
                OnLevelFinished?.Invoke();
                gameObject.SetActive(false);
                hasTriggered = true;
            }
            
        }
    }
}
