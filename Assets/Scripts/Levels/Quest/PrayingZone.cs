using UnityEngine;

public class PrayingZone : MonoBehaviour
{
    [SerializeField] private Altar altar;


    private void FixedUpdate()
    {
        Debug.Log("praying zone: " + altar.PrayingZone());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            altar.PrayingZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            altar.PrayingZone(false);
        }
    }
}
