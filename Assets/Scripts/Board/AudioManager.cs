using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;   // Singleton reference

    [Header("Audio Clips")]
    public AudioClip cardPlacementClip;

    private AudioSource audioSource;

    void Awake()
    {
        // Ensure there’s only one AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist between scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Add an AudioSource automatically
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlayCardPlacement()
    {
        if (cardPlacementClip != null)
        {
            audioSource.PlayOneShot(cardPlacementClip);
        }
    }

    // Optional helper for other sound effects
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}