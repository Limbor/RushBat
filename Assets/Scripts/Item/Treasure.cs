using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
            float angle = Random.Range(45f, 135f) / 360f * 2 * Mathf.PI;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            GameObject itemObject = Instantiate(item, transform.position + Vector3.up * 0.2f, Quaternion.identity);
            itemObject.GetComponent<Rigidbody2D>().AddForce(direction * Random.Range(5f, 8f), ForceMode2D.Impulse);
        }
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        hint.enabled = false;
    }
}
