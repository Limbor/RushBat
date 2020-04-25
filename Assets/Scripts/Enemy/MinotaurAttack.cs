using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttack : MonoBehaviour
{
    private LayerMask playerLayer;
    //攻击的范围，左上右下
    private Vector2 chopOffset1;
    private Vector2 chopOffset2;

    private Vector2 stabOffset1;
    private Vector2 stabOffset2;
    //伤害
    private int chopDamage;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        chopOffset1 = new Vector2(-0.589f, 0.731f);
        chopOffset2 = new Vector2(0.678f, -0.266f);

        chopDamage = 2;

    }
    public void animationAttack(string type)
    {
        //一次动画只能造成一次伤害
        if (!transform.GetComponent<MinotaurMovement>().Attacked())
        {
            transform.GetComponent<MinotaurMovement>().doAttack();

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
                player.GetComponent<PlayerMovement>().Hurt(damage, new Vector2(direction, 0));
                //transform.GetComponent<EnemyMovement>().getDamage(10, (int)direction * -1);
                break;
            }
        }
        
    }
}
