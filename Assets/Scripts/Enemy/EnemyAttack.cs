using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public LayerMask playerLayer;
    public Transform damagePoint;

    protected int normalDamage;
    protected float normalScope;

    protected void normalAttack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(damagePoint.position, normalScope, playerLayer);
        
        foreach (Collider2D player in players)
        {
            //根据怪物朝向施加力度
            float direction;
            if (GetComponent<EnemyMovement>().Direction())
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            
            Debug.Log("Enemy take damage, Amount: "+players.Length);
            player.GetComponent<PlayerMovement>().Hurt(normalDamage, new Vector2(direction, 0), GameManager.Enemy);
            break;
        }
    }

}
