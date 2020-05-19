using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private List<GameObject> others;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            other.GetComponent<PlayerProperty>().GetBurnt(2);
            other.GetComponent<PlayerMovement>().Hurt(2, direction * 2f, GameManager.Environment);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            int direction = transform.position.x < other.transform.position.x ? 1 : -1;
            other.GetComponent<EnemyMovement>().getDamage(100f, direction);
        }
    }
}
