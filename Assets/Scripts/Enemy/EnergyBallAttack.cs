using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallAttack : EnemyAttack
{
   
    public Transform shootPoint;
    private GameObject energyBall;

    public void shoot(){
        PoolManager.GetInstance().GetEnergyBall(shootPoint.position,transform.localScale.x);
    }

}  
