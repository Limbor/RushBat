using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttack : EnemyAttack
{
    //public Transform damagePoint;
    private bool attacked;
    private GameObject bullet;

    void Start()
    {
        normalDamage = 3;
        normalScope = 0.2f;
        //bullet = PoolManager.GetInstance().transform.GetChild(3).gameObject;
        // bullet.transform.position = shootPoint.position;
        // bullet.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //  private void shootAttack()
    // {
    //     if (!attacked)
    //     {
    //         attacked = true;
    //         Collider2D[] players = Physics2D.OverlapCircleAll(damagePoint.position, normalScope, playerLayer);

    //         foreach (Collider2D player in players)
    //         {
    //             //根据怪物朝向施加力度
    //             float direction;
    //             if (GetComponent<EnemyMovement>().Direction())
    //             {
    //                 direction = 1;
    //             }
    //             else
    //             {
    //                 direction = -1;
    //             }

    //             Debug.Log("Enemy take damage, Amount: " + players.Length);
    //             player.GetComponent<PlayerMovement>().Hurt(normalDamage, new Vector2(direction, 0), GameManager.Enemy);
    //             //transform.GetComponent<EnemyMovement>().getDamage(10, (int)direction * -1);
    //             break;
    //         }
    //     }
        
    // }

    // private void recover()
    // {
    //     attacked = false;   
    // }

  
}
