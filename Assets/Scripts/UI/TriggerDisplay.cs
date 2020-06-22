using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerDisplay : MonoBehaviour
{
    public Sprite displayImage;
    // 是否自定义图片
    public bool customizeImage;
    // 初始显示内容
    public string content = "解锁";
    // 使用碰撞题还是触发器触发
    public bool useCollision;
    
    private Image hint;
    private Text hintText;

    private void Start()
    {
        var hintUI =Instantiate(Resources.Load<GameObject>("Prefabs/UI/InteractiveHint"), transform);
        var parentScale = transform.localScale;
        hintUI.transform.localScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z);
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
