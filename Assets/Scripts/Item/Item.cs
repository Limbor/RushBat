using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : Goods
{
    // 捡起物体后的特效
    private GameObject pick;
    // 是否能够拾取
    private bool canPick;
    // 是否有拾取条件
    protected bool pickCondition = false;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        pick = Resources.Load<GameObject>("Prefabs/Item/Pick");
        canPick = false;
        if(!isGoods) StartCoroutine(CanPick());
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
    
    protected virtual void OnGround()
    {
        canPick = true;
    }
    
    private void Pick()
    {
        AudioManager.GetInstance().PlayPickAudio();
        Effect();
        Destroy(gameObject);
        Instantiate(pick, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// 宝箱或者怪物爆出效果
    /// </summary>
    /// <param name="pos">发射起点</param>
    /// <param name="randomDirection">是否随机方向发射，默认随机</param>
    /// <param name="defaultAngle">固定发射的角度，默认90度垂直向上</param>
    public void Emit(Vector3 pos, bool randomDirection = true, float defaultAngle = 90f)
    {
        transform.position = pos;
        float angle = defaultAngle;
        if (randomDirection)
        {
            angle = Random.Range(45f, 135f) / 360f * 2 * Mathf.PI;
        }
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        GetComponent<Rigidbody2D>().AddForce(direction * Random.Range(5f, 8f), ForceMode2D.Impulse);
    }
    
    // 碰撞地面后可以拾取
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            OnGround();
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
        if (isGoods || pickCondition) return;
        if (canPick && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Pick();
        }
    }
}

