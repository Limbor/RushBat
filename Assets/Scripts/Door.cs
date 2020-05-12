using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public bool needKey;
    public bool isSideDoor;
    public GameObject openDoor;

    private bool canOpen = false;
    private bool isOpen = false;

    private void Start()
    {
        GameManager.GetInstance().RegisterDoor(gameObject);
        if (isSideDoor && needKey)
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/lockSideDoor");
    }

    public void Open(bool withKey)
    {
        if (isOpen) return;
        canOpen = true;
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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (canOpen && !isOpen && needKey && InputManager.GetButtonDown("Interact"))
        {
            if (other.gameObject.GetComponent<PlayerProperty>().GetKeyNumber() == 0)
            {
                GetComponent<TriggerDisplay>().SetText("需要钥匙");
                return;
            }
            other.gameObject.GetComponent<PlayerProperty>().SetKeyNumber(-1);
            Open(true);
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
        if (!other.CompareTag("Player")) return;
        if (isOpen && !isSideDoor && InputManager.GetButtonDown("Interact"))
        {
            other.GetComponent<PlayerMovement>().EnterDoor(isSideDoor);
        }
        if (canOpen && !isOpen && needKey && InputManager.GetButtonDown("Interact"))
        {
            if (other.GetComponent<PlayerProperty>().GetKeyNumber() == 0) return;
            other.GetComponent<PlayerProperty>().SetKeyNumber(-1);
            Open(true);
        }
    }
}
