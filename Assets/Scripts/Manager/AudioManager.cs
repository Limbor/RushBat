using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager;

    private AudioSource musicAudio, playerAudio, fxAudio, ambientAudio;

    private void Awake()
    {
        if(_audioManager == null)
        {
            _audioManager = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        musicAudio = gameObject.AddComponent<AudioSource>();
        playerAudio = gameObject.AddComponent<AudioSource>();
        fxAudio = gameObject.AddComponent<AudioSource>();
        ambientAudio = gameObject.AddComponent<AudioSource>();

        musicAudio.clip = Resources.Load<AudioClip>("Sounds/BGM");
        musicAudio.playOnAwake = false;
        musicAudio.volume = 0.2f;
        musicAudio.loop = true;
        musicAudio.Play();
        
        ambientAudio.clip = Resources.Load<AudioClip>("Sounds/Ambient");
        musicAudio.playOnAwake = false;
        ambientAudio.loop = true;
        //ambientAudio.Play();
    }

    public void PlayPickAudio()
    {
        fxAudio.clip = Resources.Load<AudioClip>("Sounds/coin_04");
        fxAudio.Play();
    }

    public void PlayPowerUpAudio()
    {
        fxAudio.clip = Resources.Load<AudioClip>("Sounds/power_up_04");
        fxAudio.Play();
    }

    public void PlayJumpAudio()
    {
        playerAudio.clip = Resources.Load<AudioClip>("Sounds/jump");
        playerAudio.Play();
    }

    public static AudioManager GetInstance()
    {
        return _audioManager;
    }
}
