using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;   // Singleton 

    [Header("Audio Clips")]
    public AudioClip cardPlacementClip;
    public AudioClip BellTap;
    public AudioClip JuiceRefil;

    private AudioSource audioSource;

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlayCardPlacement() {
        if (cardPlacementClip != null)
        {
            audioSource.PlayOneShot(cardPlacementClip);
        }
    }

    public void BellTapsound() {
        if (BellTap != null)
        {
            audioSource.PlayOneShot(BellTap);
        }
    }

    public void JuiceRefilSound() {
        if (JuiceRefil != null)
        {
            audioSource.PlayOneShot(JuiceRefil);
        }
    }

    
}