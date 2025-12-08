using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;   // Singleton 

    [Header("Audio Clips")]
    public AudioClip Background;
    public AudioClip cardPlacementClip;
    public AudioClip BellTap;
    public AudioClip JuiceRefil;

    private AudioSource sfxSource;
    private AudioSource musicSource;

    void Awake(){
        
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else{
            Destroy(gameObject);
            return;
        }

        
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
    }

    public void PlayCardPlacement() {
        if (cardPlacementClip != null)
        {
            sfxSource.PlayOneShot(cardPlacementClip);
        }
    }

    public void BellTapsound() {
        if (BellTap != null)
        {
            sfxSource.PlayOneShot(BellTap);
        }
    }

    public void JuiceRefilSound() {
        if (JuiceRefil != null)
        {
            sfxSource.PlayOneShot(JuiceRefil);
        }
    }

    public void PlayBackgroundMusic() {
        if (Background != null)

        {
            musicSource.clip = Background;
            musicSource.Play();
        }
    }


    
}