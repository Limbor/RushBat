using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Treasure : MonoBehaviour
{
    // 掉落物品列表
    public GameObject[] itemList;
    
    public GameObject openedChest;

    public Image hint;

    // 触发范围显示按键提示
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        hint.enabled = true;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !InputManager.GetButtonDown("Interact")) return;
        Instantiate(openedChest).transform.position = transform.position;
        foreach (var item in itemList)
        {
            GameObject itemObject = Instantiate(item);
            itemObject.GetComponent<Item>().Emit(transform.position + Vector3.up * 0.2f);
        }
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        hint.enabled = false;
    }
}
