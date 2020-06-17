using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElementalAttack : EnemyAttack
{

    private bool attacked;

    // Start is called before the first frame update
    void Start()
    {
        normalDamage = 1;
        normalScope = 0.2f;
        attacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void fireAttack()
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
                player.GetComponent<PlayerProperty>().GetBurnt(2);
                player.GetComponent<PlayerMovement>().Hurt(normalDamage, new Vector2(direction, 0), GameManager.Enemy);
                //transform.GetComponent<EnemyMovement>().getDamage(10, (int)direction * -1);
                break;
            }
        }
        
    }

    private void recover()
    {
        attacked = false;   
    }
}
