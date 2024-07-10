using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioClip jumped;
    [SerializeField] private AudioClip hardLanding;
    [SerializeField] private AudioClip badLanded;
    [SerializeField] private AudioClip praying;
    [SerializeField] private AudioClip coin;


    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        Coin.OnCoinCollected += PlayCoinSound;
        MovementBehaviour.OnHardLandingSound += PlayHardLandingSound;
        MovementBehaviour.OnBadLandingSound += PlayBadLandSound;
    }

    private void OnDisable()
    {
        //audioSource = null;
        Coin.OnCoinCollected -= PlayCoinSound;
        Destroy(gameObject);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumped);
    }

    public void PlayHardLandingSound()
    {
        audioSource.PlayOneShot(hardLanding);
    }

    public void PlayBadLandSound()
    {
        audioSource.PlayOneShot(badLanded);
    }

    public void PlayPraySound()
    {
        audioSource.PlayOneShot(praying);
    } 

    public void PlayCoinSound()
    {
        audioSource.PlayOneShot(coin);
    }
}
