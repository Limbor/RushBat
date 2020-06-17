using System;
using UnityEngine;
using UnityEngine.UI;

public class Goods : MonoBehaviour
{
    public bool isGoods;
    
    protected int price;
    protected Text value;
    protected PlayerProperty player;
    
    private GameObject priceText;
    protected GameObject priceTextInstance;

    protected virtual void Start()
    {
        player = GameManager.GetInstance().GetPlayer().GetComponent<PlayerProperty>();
        if (!isGoods) return;
        var hint = gameObject.AddComponent<TriggerDisplay>();
        hint.content = "购买";
        priceText = Resources.Load<GameObject>("Prefabs/UI/PriceDisplay");
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        priceTextInstance = Instantiate(priceText, transform.position, Quaternion.identity);
        value = priceTextInstance.GetComponentInChildren<Text>();
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (!isGoods || !other.CompareTag("Player")) return;
        if (InputManager.GetButtonDown("Interact"))
        {
            if (player.GetCoinNumber() >= price)
            {
                Destroy(priceTextInstance);
                player.SetCoinNumber(-price);
                isGoods = false;
                if (TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
            else
            {
                GetComponent<TriggerDisplay>().SetText("金币不足");
            }
        }
    }
}