using System;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public AudioClip finishSound;
    public AudioClip coinSound;
    public AudioClip speedPotionSound;
    public AudioClip slowPotionSound;
    public AudioClip buttonClickSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
