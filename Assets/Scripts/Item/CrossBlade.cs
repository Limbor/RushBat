using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrossBlade : SurroundingItem
{
    private PlayerAttack attack;
    private float baseDamage, floatArea;
    private Queue<GameObject> enemies;

    protected override void Start()
    {
        radius = 0.8f;
        GameObject player = GameManager.GetInstance().GetPlayer();
        target = player.transform;
        base.Start();
        attack = player.GetComponent<PlayerAttack>();
        baseDamage = 10f;
        floatArea = 2f;
        enemies = new Queue<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        if (enemies.Contains(other.gameObject)) return;
        enemies.Enqueue(other.gameObject);
        StartCoroutine(ValidTarget());
        float direction = other.transform.position.x > transform.position.x ? 1f : -1f;
        float damage = baseDamage + Random.Range(-floatArea, floatArea);
        attack.Damage(other.gameObject, damage, direction);
    }
    // 没打中一个怪需要过一段时间后才能重复造成伤害
    IEnumerator ValidTarget()
    {
        yield return new WaitForSeconds(1f);
        enemies.Dequeue();
    }
}
