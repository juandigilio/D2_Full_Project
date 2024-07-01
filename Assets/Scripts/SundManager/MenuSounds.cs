using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip enterSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySelectSound()
    {
        audioSource.PlayOneShot(selectSound);
    }

    private void PlayEnterSound()
    {
        audioSource.PlayOneShot(enterSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
