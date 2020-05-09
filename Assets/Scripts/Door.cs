using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public bool needKey = false;
    public bool isSideDoor;
    public GameObject openDoor;
    
    private bool isOpen = false;
    private bool isLocked;

    private void Start()
    {
        GameManager.GetInstance().RegisterDoor(gameObject);
        isLocked = needKey;
    }

    public void Open(bool withKey)
    {
        if (isOpen) return;
        if (needKey && !withKey) return;
        isOpen = true;
        if (isSideDoor)
        {
            transform.DOMove(transform.position + Vector3.up * 1.5f, 2f).OnComplete(() =>
            {
                GetComponent<Collider>().enabled = false;
            });
        }
        else
        {
            Instantiate(openDoor, transform.position, Quaternion.identity);
            GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpen) return;
        if (!other.CompareTag("Player")) return;
        if (isSideDoor)
        {
            other.GetComponent<PlayerMovement>().EnterDoor(isSideDoor);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isOpen) return;
        if (!other.CompareTag("Player")) return;
        if (!isSideDoor && InputManager.GetButtonDown("Interact"))
        {
            other.GetComponent<PlayerMovement>().EnterDoor(isSideDoor);
        }
    }
}
