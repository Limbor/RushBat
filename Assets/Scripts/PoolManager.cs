using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager poolManager;

    public GameObject shadowPrefab;
    public GameObject dustPrefab;
    public GameObject tearPrefab;
    public GameObject firePrefab;

    public Transform moveDust;
    public Transform slideDust;

    private int shadowCount = 10;
    private int dustCount = 5;
    private int tearCount = 3;
    private int fireCount = 5;
    private Queue<GameObject> shadowPool;
    private Queue<GameObject> dustPool;
    private Queue<GameObject> tearPool;
    private Queue<GameObject> firePool;

    private void Awake()
    {
        if (poolManager == null)
        {
            poolManager = this;
            shadowPool = new Queue<GameObject>();
            dustPool = new Queue<GameObject>();
            tearPool = new Queue<GameObject>();
            firePool = new Queue<GameObject>();
            FillShadowPool();
            FillDustPool();
            FillTearPool();
            FillFirePool();
        }
    }

    public static PoolManager GetInstance()
    {
        return poolManager;
    }

    public void FillShadowPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            GameObject gameObject = Instantiate(shadowPrefab);
            gameObject.transform.SetParent(transform);
            ReturnShadowPool(gameObject);
        }
    }

    public void ReturnShadowPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        shadowPool.Enqueue(gameObject);
    }

    public void GetShadowObject()
    {
        if(shadowPool.Count == 0)
        {
            FillShadowPool();
        }
        GameObject gameObject = shadowPool.Dequeue();
        gameObject.SetActive(true);
    }

    public void FillDustPool()
    {
        for (int i = 0; i < dustCount; i++)
        {
            GameObject gameObject = Instantiate(dustPrefab);
            gameObject.transform.SetParent(transform);
            ReturnDustPool(gameObject);
        }
    }

    public void ReturnDustPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        dustPool.Enqueue(gameObject);
    }

    public void GetDustObject(bool move)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (dustPool.Count == 0)
        {
            FillDustPool();
        }
        GameObject gameObject = dustPool.Dequeue();
        if (move)
        {
            gameObject.transform.position = moveDust.transform.position;
            gameObject.transform.rotation = moveDust.transform.rotation;
            gameObject.transform.localScale = player.transform.localScale;
        }
        else
        {
            gameObject.transform.position = slideDust.transform.position;
            gameObject.transform.rotation = slideDust.transform.rotation;
            gameObject.transform.localScale = new Vector3(player.transform.localScale.x,
                slideDust.transform.localScale.y, slideDust.transform.localScale.z);
        }
        gameObject.SetActive(true);
    }

    public void FillTearPool()
    {
        for (int i = 0; i < tearCount; i++)
        {
            GameObject gameObject = Instantiate(tearPrefab);
            gameObject.transform.SetParent(transform);
            ReturnTearPool(gameObject);
        }
    }

    public void ReturnTearPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        tearPool.Enqueue(gameObject);
    }

    public void GetTearObject(Vector3 position)
    {
        if (tearPool.Count == 0)
        {
            FillTearPool();
        }
        GameObject gameObject = tearPool.Dequeue();
        gameObject.transform.position = position;
        gameObject.SetActive(true);
    }

    public void FillFirePool()
    {
        for (int i = 0; i < fireCount; i++)
        {
            GameObject gameObject = Instantiate(firePrefab);
            gameObject.transform.SetParent(transform);
            ReturnFirePool(gameObject);
        }
    }

    public void ReturnFirePool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        firePool.Enqueue(gameObject);
    }

    public void GetFireObject(Vector3 position, float direction)
    {
        if (firePool.Count == 0)
        {
            FillFirePool();
        }
        GameObject gameObject = firePool.Dequeue();
        gameObject.transform.position = position;
        gameObject.transform.localScale = new Vector3(1, direction, 1);
        gameObject.SetActive(true);
    }
}
