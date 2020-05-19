using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{
    private void Awake()
    {
        GameManager.GetInstance().RegisterExitPortal(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && InputManager.GetButtonDown("Interact"))
        {
            GameManager.GetInstance().NextLevel();
        }
    }
}
