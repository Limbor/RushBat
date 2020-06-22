using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttack : EnemyAttack
{

    public Transform shootPoint;
    private GameObject bullet;

    private GunMovement gunMovement;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void shoot(){
        PoolManager.GetInstance().GetBullet(shootPoint.position);
    }
    
     public void gunAttack(Vector3 position,int damage, float damageScope)
    {
            Collider2D[] players = Physics2D.OverlapCircleAll(position, damageScope, playerLayer);

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
                player.GetComponent<PlayerMovement>().Hurt(damage, new Vector2(direction, 0), GameManager.Enemy);
                break;
            }
     }
        

  
}
