using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // Singleton pattern for easy access

    public AudioSource musicSource;
    public AudioSource effectsSource;

    public float masterVolume = 1.0f;
    public float musicVolume = 1.0f;
    public float effectsVolume = 1.0f;

    // Sound effect clips
    public AudioClip hitSound;
    public AudioClip fallSound;
    public AudioClip jumpSound;
    public AudioClip moveSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Load saved volume settings from PlayerPrefs (or set defaults)
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1.0f);
    }

    // Play sound effects
    public void PlayHitSound()
    {
        effectsSource.PlayOneShot(hitSound, effectsVolume * masterVolume);
    }

    public void PlayFallSound()
    {
        effectsSource.PlayOneShot(fallSound, effectsVolume * masterVolume);
    }

    public void PlayJumpSound()
    {
        effectsSource.PlayOneShot(jumpSound, effectsVolume * masterVolume);
    }

    public void PlayMoveSound()
    {
        effectsSource.PlayOneShot(moveSound, effectsVolume * masterVolume);
    }

    // Set volume levels
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        musicSource.volume = volume * masterVolume;
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
        PlayerPrefs.SetFloat("EffectsVolume", volume);
    }
}
