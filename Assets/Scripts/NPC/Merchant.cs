using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var direction = transform.localScale.x;
        string[] items = {"Key", "Heart", "HalfHeart", "Shield"};
        int lastIndex = -1, index;
        for (int i = 0; i < 2; i++)
        {
            do
            {
                index = Random.Range(0, 4);
            } while (index == lastIndex);
            lastIndex = index;
            var item = Resources.Load<GameObject>("Prefabs/Item/" + items[index]);
            var itemInstance = 
                Instantiate(item, transform.position + direction * (2 + i) * Vector3.left, Quaternion.identity);
            itemInstance.GetComponent<Goods>().isGoods = true;
            var equipment = Resources.Load<GameObject>("Prefabs/Item/RandomEquipment");
            var equipmentInstance = 
                Instantiate(equipment, transform.position + direction * (4 + i) * Vector3.left, Quaternion.identity);
            equipmentInstance.GetComponent<Goods>().isGoods = true;
        }
    }
}
