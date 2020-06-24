using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{
    private bool enter = false;
    private void Awake()
    {
        GameManager.GetInstance().RegisterExitPortal(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!enter && other.CompareTag("Player") && InputManager.GetButtonDown("Interact"))
        {
            enter = true;
            GameManager.GetInstance().NextLevel();
        }
    }
}
