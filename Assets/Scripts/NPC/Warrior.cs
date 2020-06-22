using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public GameObject talkUI;

    public GameObject tip;

    private void OnTriggerEnter2D(Collider2D other) {
        tip.SetActive(true);
    }
     
    private void OnTriggerExit2D(Collider2D other) {
        tip.SetActive(false);
    }

    void Update()
    {
        if(tip.activeSelf && InputManager.GetButtonDown("Interact")){
            talkUI.SetActive(true);
            InputManager.EnterTalkingState();
        }
    }
}
