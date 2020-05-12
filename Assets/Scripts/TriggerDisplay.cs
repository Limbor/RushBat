using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerDisplay : MonoBehaviour
{
    public Sprite displayImage;
    public bool customizeImage;
    public string content = "解锁";
    public bool useCollision;
    
    private Image hint;
    private Text hintText;

    private void Start()
    {
        GameObject hintUI =
            Instantiate(Resources.Load<GameObject>("Prefabs/UI/InteractiveHint"), transform);
        hint = hintUI.GetComponentInChildren<Image>();
        hintText = hintUI.GetComponentInChildren<Text>();
        if (customizeImage) hint.sprite = displayImage;
        hintText.text = content;
        hint.enabled = false;
        hintText.enabled = false;
    }

    public void SetText(string text)
    {
        hintText.text = text;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!useCollision) return;
        if (!other.gameObject.CompareTag("Player")) return;
        hint.enabled = true;
        hintText.enabled = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!useCollision) return;
        if (!other.gameObject.CompareTag("Player")) return;
        hint.enabled = false;
        hintText.enabled = false;
        hintText.text = content;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useCollision) return;
        if (!collision.CompareTag("Player")) return;
        hint.enabled = true;
        hintText.enabled = true;
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (useCollision) return;
        if (!collision.CompareTag("Player")) return;
        hint.enabled = false;
        hintText.enabled = false;
        hintText.text = content;
    }
}
