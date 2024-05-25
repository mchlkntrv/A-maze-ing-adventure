using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; 

    public AudioClip finishSound;
    public AudioClip coinSound;
    public AudioClip speedPotionSound;
    public AudioClip slowPotionSound;
    public AudioClip buttonClickSound;
    public AudioClip backgroundMusic;

    private AudioSource audioSource;
    private AudioSource backgroundMusicSource;

    private void Awake()
    {
        if (instance == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();

            backgroundMusicSource = gameObject.AddComponent<AudioSource>();
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
            return;
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicSource.mute)
        {
            backgroundMusicSource.mute = false;
        }
        else
        {
            backgroundMusicSource.mute = true;
        }
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }

    public void PlayCollectItemSound(string itemCollectedName)
    {
        switch (itemCollectedName)
        {
            case "Coin":
                audioSource.PlayOneShot(coinSound);
                break;
            case "SpeedPotion":
                audioSource.PlayOneShot(speedPotionSound);
                break;
            case "SlowPotion":
                audioSource.PlayOneShot(slowPotionSound);
                break;
            default:
                break;
        }
    }

    public void PlayFinishSound()
    {
        audioSource.PlayOneShot(finishSound);
    }
}
