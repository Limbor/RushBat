using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttack : EnemyAttack
{
    private bool attacked;

    // Start is called before the first frame update
    void Start()
    {
        normalDamage = 1;
        normalScope = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        golemAttack();
    }

    private void golemAttack()
    {
        if (!attacked)
        {
            attacked = true;
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

                Debug.Log("Enemy take damage, Amount: " + players.Length);
                player.GetComponent<PlayerMovement>().Hurt(normalDamage, new Vector2(direction, 0), GameManager.Enemy);
                break;
            }
        }
        
    }

    private void recover()
    {
        attacked = false;   
    }
}
