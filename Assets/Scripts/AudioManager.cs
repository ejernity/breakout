using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; 

    public AudioClip brickHit;
    public AudioClip brickDestroyed;
    public AudioClip paddleHit;

    public void PlayBrickHitSound()
    {
        audioSource.PlayOneShot(brickHit);
    }

    public void PlayBrickDestroyedSound()
    {
        audioSource.PlayOneShot(brickDestroyed);
    }

    public void PlayPaddleHitSound()
    {
        audioSource.PlayOneShot(paddleHit);
    }
}
