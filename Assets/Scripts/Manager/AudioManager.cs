using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager;

    private AudioSource musicAudio, playerAudio, fxAudio, ambientAudio, environmentAudio;
    
    public AudioMixer audioMixer;


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
        environmentAudio = gameObject.AddComponent<AudioSource>();

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


     public void SetMasterVolume(Slider s)    // 控制主音量的函数
    {
        audioMixer.SetFloat("MasterVolume", s.value);
        // MasterVolume为我们暴露出来的Master的参数
    }
 
    public void SetMusicVolume(Slider s)    // 控制背景音乐音量的函数
    {
        audioMixer.SetFloat("MusicVolume", s.value);
        // MusicVolume为我们暴露出来的Music的参数
    }
 
    public void SetSoundEffectVolume(Slider s)    // 控制音效音量的函数
    {
        audioMixer.SetFloat("SoundEffectVolume", s.value);
        // SoundEffectVolume为我们暴露出来的SoundEffect的参数
    }

    public void PlayPickAudio()
    {
        fxAudio.clip = Resources.Load<AudioClip>("Sounds/Collection");
        fxAudio.Play();
    }

    public void PlayRecoverAudio()
    {
        fxAudio.clip = Resources.Load<AudioClip>("Sounds/Recovery");
        fxAudio.Play();
    }

    public void PlayPowerUpAudio()
    {
        fxAudio.clip = Resources.Load<AudioClip>("Sounds/power_up_04");
        fxAudio.Play();
    }

    public void PlayBombAudio()
    {
        fxAudio.clip = Resources.Load<AudioClip>("Sounds/bomb");
        fxAudio.Play();
    }

    public void PlayJumpAudio()
    {
        // playerAudio.clip = Resources.Load<AudioClip>("Sounds/jump");
        playerAudio.clip = Resources.Load<AudioClip>("Sounds/Jump 1");
        playerAudio.Play();
    }

    public void PlayDoorAudio()
    {
        environmentAudio.clip = Resources.Load<AudioClip>("Sounds/door_rise");
        environmentAudio.Play();
    }

    public void PlayDashAudio()
    {
        playerAudio.clip = Resources.Load<AudioClip>("Sounds/dash");
        playerAudio.Play();
    }
    
     public void PlayHitAudio()
    {
        playerAudio.clip = Resources.Load<AudioClip>("Sounds/hit");
        playerAudio.Play();
    }

    public void PlaySlashAudio()
    {
        playerAudio.clip = Resources.Load<AudioClip>("Sounds/slash");
        playerAudio.Play();
    }

    public static AudioManager GetInstance()
    {
        return _audioManager;
    }
}
