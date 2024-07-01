using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioClip jumped;
    [SerializeField] private AudioClip badLanded;
    [SerializeField] private AudioClip praying;


    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumped);
    }

    public void PlayBadLandSound()
    {
        audioSource.PlayOneShot(badLanded);
    }

    public void PlayPraySound()
    {
        audioSource.PlayOneShot(praying);
    }

    public void PlayAltarSound()
    {
        audioSource.PlayOneShot(praying);
    }

    public void PlayDoorSound()
    {
        audioSource.PlayOneShot(praying);
    }
}
