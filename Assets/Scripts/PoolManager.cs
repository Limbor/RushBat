using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager poolManager;

    public GameObject shadowPrefab;

    private int shadowCount = 10;
    private Queue<GameObject> pool;


    private void Awake()
    {
        if (poolManager == null)
        {
            poolManager = this;
            pool = new Queue<GameObject>();
            FillPool();
        }
    }

    public static PoolManager getInstance()
    {
        return poolManager;
    }

    public void FillPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            GameObject gameObject = Instantiate(shadowPrefab);
            gameObject.transform.SetParent(transform);
            ReturnPool(gameObject);
        }
    }

    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }

    public void GetObject()
    {
        if(pool.Count == 0)
        {
            FillPool();
        }
        GameObject gameObject = pool.Dequeue();
        gameObject.SetActive(true);
    } 
}
