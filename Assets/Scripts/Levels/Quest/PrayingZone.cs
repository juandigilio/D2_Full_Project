using UnityEngine;

public class PrayingZone : MonoBehaviour
{
    [SerializeField] private Altar altar;
    [SerializeField] private Canvas canvas;


    private void Awake()
    {
        if (canvas != null)
        {
            canvas.enabled = false;
        }
    }

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
