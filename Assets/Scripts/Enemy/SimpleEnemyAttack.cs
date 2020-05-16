using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyAttack : EnemyAttack
{
    /*
       这是简单的怪物的攻击脚本，怪物攻击通过DamagePoint进行检测，
       要设置的参数有攻击伤害，攻击范围半径
       并在怪物的attack动画中要造成伤害的所有帧添加事件simpleEnemyAttack(), 在最后一帧添加事件recover();
     */
    private bool attacked;
    public int Damage;
    public float Scope;
 
    void Start()
    {
        attacked = false;
        normalDamage = Damage;
        normalScope = Scope;
    }
    
    private void simpleEnemyAttack()
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
