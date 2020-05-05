using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttack : MonoBehaviour
{
    private bool attacked;

    private LayerMask playerLayer;
    //攻击的范围，左上右下
    private Vector2 chopOffset1;
    private Vector2 chopOffset2;
    private Vector2 stabOffset1;
    private Vector2 stabOffset2;
    private Vector2 rotateOffset1;
    private Vector2 rotateOffset2;
    //伤害
    private int chopDamage;
    private int stabDamage;
    private int rotateDamage;

    void Start()
    {
        attacked = false;

        playerLayer = 1 << LayerMask.NameToLayer("Player");
        chopOffset1 = new Vector2(-1f, 1.16f);
        chopOffset2 = new Vector2(1.06f, -0.42f);
        stabOffset1 = new Vector2(0f, 0.1f);
        stabOffset2 = new Vector2(1.1f, -0.24f);
        rotateOffset1 = new Vector2(-1.25f, 0);
        rotateOffset2 = new Vector2(1.1f, -0.42f);
        chopDamage = 2;
        stabDamage = 1;
        rotateDamage = 1;

    }

    public void animationAttack(string type)
    {
        //一次动画只能造成一次伤害
        if (!attacked)
        {
            attacked = true;

            Vector2 offset1 = Vector2.zero, offset2 = Vector2.zero;
            int damage = 0;
            switch (type)
            {
                //根据不同的攻击类型设置范围和伤害
                case "chop":
                    offset1 = chopOffset1;
                    offset2 = chopOffset2;
                    damage = chopDamage;
                    break;
                case "stab":
                    offset1 = stabOffset1;
                    offset2 = stabOffset2;
                    damage = stabDamage;
                    break;
                case "rotate":
                    offset1 = rotateOffset1;
                    offset2 = rotateOffset2;
                    damage = rotateDamage;
                    break;
            }

            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] players = Physics2D.OverlapAreaAll(position + offset1, position + offset2, playerLayer);
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
                //transform.GetComponent<EnemyMovement>().getDamage(10, (int)direction * -1);
                break;
            }
        }
        
    }

    private void canAttack()
    {
        attacked = false;
    }
}
