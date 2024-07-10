using UnityEngine;
using System;

public class PrayingZone : MonoBehaviour
{
    [SerializeField] private Altar altar;

    public static Action OnPrayingCanvasOn;
    public static Action OnPrayingCanvasOff;


    /// <summary>
    /// Activates the praying zone and enables the canvas when the player enters the trigger zone.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            altar.PrayingZone(true);
            OnPrayingCanvasOn?.Invoke();
        }
    }

    /// <summary>
    /// Deactivates the praying zone and disables the canvas when the player exits the trigger zone.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            altar.PrayingZone(false);
            OnPrayingCanvasOff?.Invoke();
        }
    }
}
