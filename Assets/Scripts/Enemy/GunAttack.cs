using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttack : EnemyAttack
{

    public Transform shootPoint;
    private GameObject bullet;

    public void shoot(){
        PoolManager.GetInstance().GetBullet(shootPoint.position,transform.localScale.x);
    }
  
}
