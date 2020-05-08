using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool needKey = false;
    public bool isSideDoor;
    public GameObject openDoor;
    
    private bool isOpen = false;

    private void Start()
    {
        GameManager.GetInstance().RegisterDoor(gameObject);
    }

    public void Open(bool withKey)
    {
        if (isOpen) return;
        if (needKey && !withKey) return;
        isOpen = true;
        if (isSideDoor)
        {
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            Instantiate(openDoor, transform.position, Quaternion.identity);
            GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isOpen) return;
        if (!other.CompareTag("Player")) return;
        if (isSideDoor)
        {
            other.GetComponent<PlayerMovement>().EnterDoor();
        }
        else if (InputManager.GetButtonDown("Interact"))
        {
            other.GetComponent<PlayerMovement>().EnterDoor();
        }
    }
}
