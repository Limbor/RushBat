using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager poolManager;

    public GameObject shadowPrefab;
    public GameObject dustPrefab;
    public GameObject wallDustPrefab;
    public GameObject tearPrefab;
    public GameObject firePrefab;
    public GameObject woodSpikePrefab;

    private int shadowCount = 10;
    private int dustCount = 3;
    private int tearCount = 3;
    private int fireCount = 3;
    private int spikeCount = 3;
    private Queue<GameObject> shadowPool;
    private Queue<GameObject> dustPool;
    private Queue<GameObject> wallDustPool;
    private Queue<GameObject> tearPool;
    private Queue<GameObject> firePool;
    private Queue<GameObject> spikePool;

    private void Awake()
    {
        if (poolManager == null)
        {
            poolManager = this;
            shadowPool = new Queue<GameObject>();
            dustPool = new Queue<GameObject>();
            wallDustPool = new Queue<GameObject>();
            tearPool = new Queue<GameObject>();
            firePool = new Queue<GameObject>();
            spikePool = new Queue<GameObject>();
            FillShadowPool();
            FillDustPool(true);
            FillDustPool(false);
            FillTearPool();
            FillFirePool();
            FillSpikePool();
            return;
        }
        Destroy(gameObject);
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

    public void FillDustPool(bool move)
    {
        if (move)
        {
            for (int i = 0; i < dustCount; i++)
            {
                GameObject gameObject = Instantiate(dustPrefab);
                gameObject.transform.SetParent(transform);
                ReturnDustPool(gameObject, move);
            }
        }
        else
        {
            for (int i = 0; i < dustCount; i++)
            {
                GameObject gameObject = Instantiate(wallDustPrefab);
                gameObject.transform.SetParent(transform);
                ReturnDustPool(gameObject, move);
            }
        }
    }

    public void ReturnDustPool(GameObject gameObject, bool move)
    {
        gameObject.SetActive(false);
        if (move) dustPool.Enqueue(gameObject);
        else wallDustPool.Enqueue(gameObject);
    }

    public void GetDustObject(bool move)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject gameObject;
        if (move)
        {
            if (dustPool.Count == 0) FillDustPool(move);
            gameObject = dustPool.Dequeue();
            gameObject.transform.position = player.transform.position;
            gameObject.transform.rotation = player.transform.rotation;
            gameObject.transform.localScale = player.transform.localScale;
        }
        else
        {
            if (wallDustPool.Count == 0) FillDustPool(move);
            gameObject = wallDustPool.Dequeue();
            gameObject.transform.position = player.transform.position;
            gameObject.transform.rotation = player.transform.rotation;
            gameObject.transform.localScale = player.transform.localScale;
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

    public void FillSpikePool()
    {
        for (int i = 0; i < spikeCount; i++)
        {
            GameObject gameObject = Instantiate(woodSpikePrefab);
            gameObject.transform.SetParent(transform);
            ReturnSpikePool(gameObject);
        }
    }

    public void ReturnSpikePool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        spikePool.Enqueue(gameObject);
    }

    public void GetSpikeObject(Transform transform)
    {
        if (spikePool.Count == 0)
        {
            FillSpikePool();
        }
        GameObject gameObject = spikePool.Dequeue();
        gameObject.transform.position = transform.position;
        gameObject.transform.localScale = transform.localScale;
        gameObject.SetActive(true);
    }
}
