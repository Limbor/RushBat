using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour
{
    // 捡起物体后的特效
    private GameObject pick;
    // 是否能够拾取
    private bool canPick;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        pick = Resources.Load<GameObject>("Prefabs/Item/Pick");
        canPick = false;
        StartCoroutine(CanPick());
    }

    // 0.5s后可以拾取
    IEnumerator CanPick()
    {
        yield return new WaitForSeconds(0.5f);
        canPick = true;
    }

    // 物品效果
    protected virtual void Effect()
    {
        
    }
    
    private void Pick()
    {
        Effect();
        Destroy(gameObject);
        Instantiate(pick, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// 宝箱或者怪物爆出效果
    /// </summary>
    /// <param name="pos">发射起点</param>
    /// <param name="randomDirction">是否随机方向发射，默认随机</param>
    /// <param name="defaultAngle">固定发射的角度，默认90度垂直向上</param>
    public void Emit(Vector3 pos, bool randomDirction = true, float defaultAngle = 90f)
    {
        transform.position = pos;
        float angle = defaultAngle;
        if (randomDirction)
        {
            angle = Random.Range(45f, 135f) / 360f * 2 * Mathf.PI;
        }
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        GetComponent<Rigidbody2D>().AddForce(direction * Random.Range(5f, 8f), ForceMode2D.Impulse);
    }
    
    // 碰撞地面后可以拾取
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
        canPick = true;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (canPick && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Pick();
        }
    }
}

