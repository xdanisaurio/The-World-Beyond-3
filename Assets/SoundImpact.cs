using UnityEngine;

public class SoundImpact : MonoBehaviour
{
    public AudioClip impactSFX;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && impactSFX != null)
        {
            audioSource.PlayOneShot(impactSFX);
        }
    }
}

