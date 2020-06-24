using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunMovement : EnemyMovement
{
  //控制怪物行动范围
    public Transform startPos;
    public Transform endPos;

    private float startx;
    private float endx;

    private float attackInterval;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        startx = startPos.position.x;
        endx = endPos.position.x;
        maxBlood = 60;
        blood = maxBlood;
        walkspeed = 50f;
      
        length = 0.2f;
        faceright = true;
        attackInterval = 0f;


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
            rb.velocity = new Vector2(0, rb.velocity.y);
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
            float heightdis = Mathf.Abs(player.transform.position.y - transform.position.y);
            if (faceright && distance > 0 && distance < 2 && heightdis < 0.5)
            {
                attacking = true;

                anim.SetBool("run", false);
                
                anim.SetBool("attack", true);

                Invoke("finishAttack", 1.5f);

            }
            else if (!faceright && distance < 0 && distance >-2&& heightdis < 0.5)
            {
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
        attackInterval = 1.5f;
    }
    protected override void Drop()
    {
        int silverNum = Random.Range(2, 4);

        for (int i = 1; i <= silverNum; i++)
        {
            GameObject itemObject = Instantiate(silverPrefab);
            itemObject.GetComponent<Item>().Emit(transform.position + Vector3.up * 0.2f);
        }

        float heartDrop = Random.Range(0, 1f);
        if (heartDrop <= 0.2f)
        {
            GameObject itemObject = Instantiate(heartPrefab);
            itemObject.GetComponent<Item>().Emit(transform.position + Vector3.up * 0.2f);
        }
    }

}
