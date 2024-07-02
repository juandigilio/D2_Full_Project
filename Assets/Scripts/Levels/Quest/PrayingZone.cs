using UnityEngine;

public class PrayingZone : MonoBehaviour
{
    [SerializeField] private Altar altar;
    [SerializeField] private Canvas canvas;

    /// <summary>
    /// Disables the canvas if it is not null.
    /// </summary>
    private void Awake()
    {
        if (canvas != null)
        {
            canvas.enabled = false;
        }
    }

    /// <summary>
    /// Activates the praying zone and enables the canvas when the player enters the trigger zone.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            altar.PrayingZone(true);

            if (canvas != null)
            {
                canvas.enabled = true;
            }
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

            if (canvas != null)
            {
                canvas.enabled = false;
            }
        }
    }
}
