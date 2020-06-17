using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyMovement : EnemyMovement
{
    /*
      简单怪物的行为脚本，怪物的动画只有idle, run, atack, die，也可以有hurt动画，
      怪物的状态bool变量有四个：run, attack, hurt, dead
      怪物的行为模式为在一定范围内左右巡逻，玩家进入移动范围后会向玩家移动，
      接近玩家后会攻击，怪物有攻击间隔
      要设置的参数为移动范围，攻击间隔，血量，移速，长度(transform.position到身体前侧的水平距离,
      用于检测前方障碍), 银币数和心的掉落几率，以及EnemyMovement中的参数
     */
    public Transform startPos;
    public Transform endPos;

    private float startx;
    private float endx;

    public float AttackInterval;
    private float attackInterval;
    public float Blood;
    public float Speed;
    public float Length;
    public int SilverNum;
    public float HeartRate;

    void Start()
    {
        base.Start();
        startx = startPos.position.x;
        endx = endPos.position.x;
        maxBlood = Blood;
        blood = maxBlood;
        walkspeed = Speed;

        length = Length;
        faceright = true;
        attackInterval = AttackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (isdead) return;
        base.Update();
        Move();
        Attack();
    }

    void Move()
    {
        if (isdead)
        {
            return;
        }
        if (attacking)
        {
            // Debug.Log("Attacking");
            rb.velocity = new Vector2(0, rb.velocity.y);
            //Debug.Log(rb.velocity);
            return;
        }

        //到起点和终点的距离
        float edis = endx - transform.position.x;
        float sdis = transform.position.x - startx;
        float hdis = Mathf.Abs(player.transform.position.y - transform.position.y);

        if (faceright && edis > 0)
        {
            //面向右，未到达终点
            if (player.transform.position.x > startx && player.transform.position.x < transform.position.x)
            {
                //玩家在身后，转身
                if (!attacking && hdis < 0.5 && !block)
                {
                    Flip();
                }
            }
            else
            {
                anim.SetBool("run", true);
                rb.velocity = new Vector2(walkspeed * Time.deltaTime, rb.velocity.y);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, length + 0.05f, groundLayer.value);
                if (hit)
                {
                    block = true;
                    Flip();
                    rb.velocity = new Vector2(-1 * walkspeed * Time.deltaTime, rb.velocity.y);
                }
                else
                {
                    block = false;
                }
            }
        }
        else if (faceright && edis <= 0)
        {
            //面向右，到达终点
            anim.SetBool("run", false);
            rb.velocity = new Vector2(0, 0);
            if (waittime > 0)
            {
                waittime -= Time.deltaTime;
            }
            else
            {
                waittime = 1;
                Flip();
            }
        }
        else if (!faceright && sdis > 0)
        {
            if (player.transform.position.x > transform.position.x && player.transform.position.x < endx)
            {
                if (!attacking && hdis < 0.5 && !block)
                {
                    Flip();
                }
            }
            else
            {
                anim.SetBool("run", true);
                rb.velocity = new Vector2(-1 * walkspeed * Time.deltaTime, rb.velocity.y);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, length + 0.05f, groundLayer.value);
                if (hit)
                {
                    block = true;
                    Flip();
                    rb.velocity = new Vector2(walkspeed * Time.deltaTime, rb.velocity.y);
                }
                else
                {
                    block = false;
                }
            }
        }
        else
        {
            anim.SetBool("run", false);
            rb.velocity = new Vector2(0, 0);
            if (waittime > 0)
            {
                waittime -= Time.deltaTime;
            }
            else
            {
                waittime = 1;
                Flip();
            }
        }
    }

    void Attack()
    {
        attackInterval -= Time.deltaTime;
        if (!isdead && ground && !hurt)
        {
            //根据和玩家的距离进行攻击
            float distance = player.transform.position.x - transform.position.x;
            //Debug.Log(distance);
            float heightdis = Mathf.Abs(player.transform.position.y - transform.position.y);
            if (faceright && distance > 0 && distance < 0.5 && heightdis < 0.5)
            {
                // Debug.Log("Attack!");
                attacking = true;

                anim.SetBool("run", false);

                anim.SetBool("attack", true);

                Invoke("finishAttack", 1.5f);

            }
            else if (!faceright && distance < 0 && distance > -0.5 && heightdis < 0.5)
            {
                // Debug.Log("Attack!");
                attacking = true;

                anim.SetBool("run", false);


                anim.SetBool("attack", true);

                Invoke("finishAttack", 1.5f);

            }

        }
    }

    public void finishAttack()
    {
        anim.SetBool("attack", false);
        attacking = false;
        attackInterval = AttackInterval;
    }
    protected override void Drop()
    {
        int silverNum = SilverNum;

        for (int i = 1; i <= silverNum; i++)
        {
            GameObject itemObject = Instantiate(silverPrefab);
            itemObject.GetComponent<Item>().Emit(transform.position + Vector3.up * 0.2f);
            //item.
            //itemList.Add(Instantiate())
        }

        float heartDrop = Random.Range(0, 1);
        if (heartDrop <= HeartRate)
        {
            GameObject itemObject = Instantiate(silverPrefab);
            itemObject.GetComponent<Item>().Emit(transform.position + Vector3.up * 0.2f);
        }
    }
}
