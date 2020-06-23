using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager poolManager;
    private GameObject player;

    public GameObject shadowPrefab;
    public GameObject dustPrefab;
    public GameObject wallDustPrefab;
     public GameObject tearPrefab;
    public GameObject firePrefab;
    public GameObject woodSpikePrefab;
    public GameObject damageTextPrefab;
    public GameObject dartPrefab;
    public GameObject bulletPrefab;
    public GameObject energyBallPrefab;

    private int shadowCount = 10;
    private int dustCount = 3;
    private int tearCount = 3;
    private int fireCount = 3;
    private int spikeCount = 3;
    private int damageTextCount = 3;
    private int bulletCount = 3;
    private int energyBallCount = 3;
    
    private Queue<GameObject> shadowPool;
    private Queue<GameObject> dustPool;
    private Queue<GameObject> wallDustPool;
    private Queue<GameObject> tearPool;
    private Queue<GameObject> firePool;
    private Queue<GameObject> spikePool;
    private Queue<GameObject> damageTextPool;
    private Queue<GameObject> dartPool;
    private Queue<GameObject> bulletPool;
    private Queue<GameObject> energyBallPool;

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
            damageTextPool = new Queue<GameObject>();
            dartPool = new Queue<GameObject>();
            bulletPool = new Queue<GameObject>();
            energyBallPool = new Queue<GameObject>();
            FillShadowPool();
            FillDustPool(true);
            FillDustPool(false);
            FillTearPool();
            FillFirePool();
            FillSpikePool();
            FillDamageTextPool();
            FillBulletPool();
            FillEnergyBallPool();
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        player = GameManager.GetInstance().GetPlayer();
    }

    public static PoolManager GetInstance()
    {
        return poolManager;
    }

    public void FillShadowPool()
    {
        for (int i = 0; i < shadowCount; i++)
        {
            GameObject gameObject = Instantiate(shadowPrefab, transform);
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
                GameObject gameObject = Instantiate(dustPrefab, transform);
                ReturnDustPool(gameObject, move);
            }
        }
        else
        {
            for (int i = 0; i < dustCount; i++)
            {
                GameObject gameObject = Instantiate(wallDustPrefab, transform);
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

    public void FillFirePool()
    {
        for (int i = 0; i < fireCount; i++)
        {
            GameObject gameObject = Instantiate(firePrefab, transform);
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
            GameObject gameObject = Instantiate(woodSpikePrefab, transform);
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
    
    public void FillDamageTextPool()
    {
        for (int i = 0; i < damageTextCount; i++)
        {
            GameObject gameObject = Instantiate(damageTextPrefab, transform);
            ReturnDamageTextPool(gameObject);
        }
    }

    public void ReturnDamageTextPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        damageTextPool.Enqueue(gameObject);
    }

    /// <summary>
    /// 显示伤害数字
    /// </summary>
    /// <param name="position">伤害显示位置</param>
    /// <param name="damage">伤害数值</param>
    /// <param name="type">显示类型</param>
    public void GetDamageText(Vector3 position, float damage, int type = 1)
    {
        if(damageTextPool.Count == 0)
        {
            FillDamageTextPool();
        }
        Color[] colors = {Color.white, Color.yellow, new Color(1, 0.71f, 0), Color.red};
        GameObject gameObject = damageTextPool.Dequeue();
        gameObject.transform.position = position;
        var text = gameObject.GetComponent<DamageText>();
        text.SetNumber(damage);
        text.SetColor(colors[type - 1]);
        if(type == 4) text.SetScale();
        gameObject.SetActive(true);
    }

    public void GetDart(Vector3 pos)
    {
        if (dartPool.Count == 0)
        {
            dartPool.Enqueue(Instantiate(dartPrefab, transform));
        }
        var dart = dartPool.Dequeue();
        dart.transform.position = pos;
        dart.SetActive(true);
    }

    public void ReturnDartPool(GameObject dart)
    {
        dart.SetActive(false);
        dartPool.Enqueue(dart);
    }

    
    public void FillTearPool()
    {
        for (int i = 0; i < tearCount; i++)
        {
            GameObject gameObject = Instantiate(tearPrefab, transform);
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

      public void GetBullet(Vector3 pos, float direction)
    {
        if (bulletPool.Count == 0)
        {
            FillBulletPool();
        }
        GameObject gameObject = bulletPool.Dequeue();
        gameObject.transform.position = pos;
        gameObject.SetActive(true);
        Vector3 theScale = gameObject.transform.localScale;
        theScale.x *= direction;
        gameObject.transform.localScale = theScale;
    }

    public void ReturnBulletPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

      public void FillBulletPool()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject gameObject = Instantiate(bulletPrefab, transform);
            ReturnBulletPool(gameObject);
        }
    }

       public void GetEnergyBall(Vector3 pos,float direction)
    {
        if (energyBallPool.Count == 0)
        {
            FillEnergyBallPool();
        }
        GameObject energyBall = energyBallPool.Dequeue();
        energyBall.transform.position = pos;
        Vector3 theScale = gameObject.transform.localScale;
        theScale.x *= direction;
        energyBall.transform.localScale = theScale;
        energyBall.SetActive(true);
    }

    public void ReturnEnergyBallPool(GameObject energyBall)
    {
        energyBall.SetActive(false);
        energyBallPool.Enqueue(energyBall);
    }

       public void FillEnergyBallPool()
    {
        for (int i = 0; i < energyBallCount; i++)
        {
            GameObject gameObject = Instantiate(energyBallPrefab, transform);
            ReturnEnergyBallPool(gameObject);
        }
    }
}
