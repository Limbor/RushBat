﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    public GameObject Button;
    public GameObject talkUI;

    private void Awake(){
        Button.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Button.SetActive(false);
    }

    private void Update()
    {
        if (Button.activeSelf && InputManager.GetButtonDown("Interact"))
        {
             talkUI.SetActive(true);
             InputManager.EnterTalkingState();
        }
    }
}
