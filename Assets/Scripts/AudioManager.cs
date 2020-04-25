using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioManager audioManager;

    private AudioSource musicAudio, playerAudio, fxAudio, ambientAudio;

    private void Awake()
    {
        if(audioManager == null)
        {
            audioManager = this;
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
        // musicAudio.Play();
        ambientAudio.clip = Resources.Load<AudioClip>("Sounds/Ambient");
        ambientAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
