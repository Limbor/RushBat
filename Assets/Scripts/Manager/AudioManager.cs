using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager;
    public AudioSource musicAudio, playerAudio, fxAudio, ambientAudio, environmentAudio;
    public AudioMixer audioMixer;
    public Slider master,music,soundEffect;


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
       musicAudio.Play();
    }


    public float GetMasterVolume()
    {
        if(audioMixer.GetFloat("MasterVolume", out float volume)) return volume;
        return 0;
    }

    public void SetMasterVolume(float volume)    // 控制主音量的函数
    {
        audioMixer.SetFloat("MasterVolume", volume);
        // MasterVolume为我们暴露出来的Master的参数
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

    public void MasterVolumeToggle(Toggle t){
        if(!t.isOn){
            audioMixer.SetFloat("MasterVolume", -40);
            master.value=-40;
            music.value=-40;
            soundEffect.value=-40;
        }
        else {
            audioMixer.SetFloat("MasterVolume", 0);
            master.value=1;
            music.value=1;
            soundEffect.value=1;
        }

    }

    
    public void MusicVolumeToggle(Toggle t){
        if(!t.isOn){
            audioMixer.SetFloat("MusicVolume", -40);
            music.value=-40;
        }
        else {
            audioMixer.SetFloat("MusicVolume", 0);
            music.value=1;
        }

    }

    
    public void SoundEffectVolumeToggle(Toggle t){
        if(!t.isOn){
            audioMixer.SetFloat("SoundEffectVolume", -40);
            soundEffect.value=-40;
        }
        else {
            audioMixer.SetFloat("SoundEffectVolume", 0);
            soundEffect.value=1;
        }

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
