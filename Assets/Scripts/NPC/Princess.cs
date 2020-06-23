using System;
using UnityEngine;

public class Princess : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && InputManager.GetButtonDown("Interact"))
        {
            GameManager.GetInstance().LevelComplete();
        }
    }
}