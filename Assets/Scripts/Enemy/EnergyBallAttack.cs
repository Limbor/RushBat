using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallAttack : EnemyAttack
{
   
    public Transform shootPoint;

    private GameObject energyBall;

    private GunMovement gunMovement;

     private bool attacked;

    
    void Start()
    {
        attacked=false;
    }

    void Update()
    {
        
    }

    public void shoot(){
        PoolManager.GetInstance().GetEnergyBall(shootPoint.position);
    }
    
     public void gunAttack(Vector3 position,int damage, float damageScope)
    {
        if(!attacked){
            attacked=true;

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

     
    private void recover()
    {
        attacked = false;   
    }
}  
