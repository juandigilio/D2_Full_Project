using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip wallSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySelectSound()
    {
        audioSource.PlayOneShot(selectSound);
    }

    public void PlayEnterSound()
    {
        audioSource.PlayOneShot(enterSound);
    }

    public void PlayWallSound()
    {
        audioSource.clip = wallSound;
        audioSource.Play();
    }

    public void StopWallSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

}
