using UnityEngine;

public class AltarSound : MonoBehaviour
{
    [SerializeField] private AudioClip altarUp;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAltarSound()
    {
        audioSource.PlayOneShot(altarUp);
    }


}
