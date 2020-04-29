using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : MonoBehaviour
{
    public GameObject[] itemList;
    public GameObject openedChest;

    public Image hint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        hint.enabled = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !InputManager.GetKeyDown(KeyCode.F)) return;
        Instantiate(openedChest).transform.position = transform.position;
        foreach (var item in itemList)
        {
            Instantiate(item).transform.position = transform.position;
        }
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        hint.enabled = false;
    }
}
