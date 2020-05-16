using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Treasure : MonoBehaviour
{
    // 掉落物品列表
    private GameObject goldCoin, silverCoin, shield, key, heart, halfHeart;
    private bool open;
    private bool isOpening;

    public bool locked;
    public bool random;
    public List<GameObject> itemList;
    public GameObject openedChest;

    private void Start()
    {
        if (locked) GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/LockChest");
        goldCoin = Resources.Load<GameObject>("Prefabs/Item/GoldCoin");
        silverCoin = Resources.Load<GameObject>("Prefabs/Item/SilverCoin");
        shield = Resources.Load<GameObject>("Prefabs/Item/Shield");
        key = Resources.Load<GameObject>("Prefabs/Item/Key");
        heart = Resources.Load<GameObject>("Prefabs/Item/Heart");
        halfHeart = Resources.Load<GameObject>("Prefabs/Item/HalfHeart");
        if(random) RandomItems();
    }

    private void RandomItems()
    {
        itemList.Clear();
        int randomCoin = Random.Range(0, 20);
        if (randomCoin < 10 || locked)
        {
            randomCoin = randomCoin % 4 + 4;
            if (randomCoin >= 5)
            {
                itemList.Add(goldCoin);
                randomCoin -= 5;
            }
            for (int i = 0; i < randomCoin; i++)
            {
                itemList.Add(silverCoin);
            }
        }
        int randomShield = Random.Range(0, 4);
        if(randomShield == 0 || locked && randomShield < 2) itemList.Add(shield);
        int randomHeart = Random.Range(0, 20);
        if (randomHeart < 10)
        {
            if(randomHeart % 2 == 1 || locked) itemList.Add(heart);
            else itemList.Add(halfHeart);
        }
        if(Random.Range(0, 3) == 0) itemList.Add(key);
        if (itemList.Count > 0) return;
        for (int i = 0; i < 4; i++)
        {
            itemList.Add(silverCoin);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (open || isOpening) return;
        if (!collision.CompareTag("Player") || !InputManager.GetButtonDown("Interact")) return;
        isOpening = true;
        transform.DOShakeRotation(0.5f, 30f, 30).OnComplete(() =>
        {
            if (locked)
            {
                if (collision.GetComponent<PlayerProperty>().GetKeyNumber() == 0)
                {
                    GetComponent<TriggerDisplay>().SetText("需要钥匙");
                    isOpening = false;
                    return;
                }
                collision.GetComponent<PlayerProperty>().SetKeyNumber(-1);
            }
            open = true;
            isOpening = false;
            Instantiate(openedChest).transform.position = transform.position;
            foreach (var item in itemList)
            {
                GameObject itemObject = Instantiate(item);
                itemObject.GetComponent<Item>().Emit(transform.position + Vector3.up * 0.3f);
            }

            if (Random.Range(0, 1f) < 0.05f || locked && Random.Range(0, 1f) < 0.1f)
            {
                var equipment = 
                    Instantiate(Resources.Load<GameObject>("Prefabs/Item/RandomEquipment"));
                equipment.transform.position = transform.position +  Vector3.up * 0.3f;
            }
            Destroy(gameObject);
        });
    }
}
