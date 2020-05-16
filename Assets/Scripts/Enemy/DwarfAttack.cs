using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfAttack : EnemyAttack
{
    private bool damaged;

    //攻击范围，左上右下
    private Vector2 slashOffset1;
    private Vector2 slashOffset2;
    private Vector2 hackOffset1;
    private Vector2 hackOffset2;
    private Vector2 spinOffset1;
    private Vector2 spinOffset2;

    //伤害
    private int slashDamage;
    private int hackDamage;
    private int spinDamage;

    void Start()
    {
        damaged = false;
        slashDamage = 1;
        hackDamage = 2;
        spinDamage = 1;

        slashOffset1 = new Vector2(0, 0);
        slashOffset2 = new Vector2(0.72f, -0.47f);
        hackOffset1 = new Vector2(-0.1f, 0.48f);
        hackOffset2 = new Vector2(0.54f, -0.3f);
        spinOffset1 = new Vector2(-0.5f, 0.1f);
        spinOffset2 = new Vector2(0.4f, -0.48f);
    }

    void animationAttack(string type)
    {
        if (!damaged)
        {
            damaged = true;
            Vector2 offset1 = Vector2.zero;
            Vector2 offset2 = Vector2.zero;
            int damage = 0;
            float direction = GetComponent<EnemyMovement>().Direction() ? 1 : -1;

            switch (type)
            {
                case "slash":
                    offset1 = slashOffset1;
                    offset2 = slashOffset2;
                    damage = slashDamage;
                    break;
                case "hack":
                    offset1 = hackOffset1;
                    offset2 = hackOffset2;
                    damage = hackDamage;
                    break;
                case "spin":
                    offset1 = spinOffset1;
                    offset2 = spinOffset2;
                    damage = spinDamage;
                    break;
            }
            offset1.x *= direction;
            offset2.x *= direction;

            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] players = Physics2D.OverlapAreaAll(position + offset1, position + offset2, playerLayer);
            foreach (Collider2D player in players)
            {
                //Debug.Log("Enemy take damage, Amount: " + players.Length);
                player.GetComponent<PlayerMovement>().Hurt(damage, new Vector2(direction, 0), GameManager.Enemy);
                //transform.GetComponent<EnemyMovement>().getDamage(10, (int)direction * -1);
                break;
            }
        }
    }

    private void canDamage()
    {
        damaged = false;
    }
}
