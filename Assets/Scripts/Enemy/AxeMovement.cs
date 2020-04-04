using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeMovement : EnemyMovement
{
    //控制怪物行动范围
    public Transform startPos;
    public Transform endPos;

    private float startx;
    private float endx;

    //private bool walking;

    // Start is called before the first frame update
    void Start()
    {
        
        base.Start();
        
        anim.SetBool("jump", false);
        anim.SetBool("dead", false);
        anim.SetBool("walk", false);
        startx = startPos.position.x;
        endx = endPos.position.x;
        blood = 100;
        walkspeed = 100f;
        waittime = 1f;
        faceright = true;
        //walking = true;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        Attack();
        //testAttack();
      
    }
    //public bool attack;
    //void testAttack()
    //{
    //    attacking = attack;
    //    if (attack)
    //    {
    //        anim.SetBool("walk", false);
    //        anim.SetBool("attack", true);
    //    }
       
    //}
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        if (isdead || attacking)
        {
            //Debug.Log("Attacking");
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
                if (!attacking && hdis < 0.5) 
                {
                    Flip();
                }
            }
            else
            {
                anim.SetBool("walk", true);
                rb.velocity = new Vector2(walkspeed * Time.deltaTime, rb.velocity.y);
            }
        }
        else if (faceright && edis <= 0)
        {
            //面向右，到达终点
            anim.SetBool("walk", false);
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
                if (!attacking && hdis < 0.5) 
                {
                    Flip();
                }
            }
            else
            {
                anim.SetBool("walk", true);
                rb.velocity = new Vector2(-1 * walkspeed * Time.deltaTime, rb.velocity.y);
            }
        }
        else
        {
            anim.SetBool("walk", false);
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
        if (!isdead && ground)
        {
            //根据和玩家的距离进行攻击
            float distance = player.transform.position.x - transform.position.x;
            //Debug.Log(distance);
            float heightdis = Mathf.Abs(player.transform.position.y - transform.position.y);
            if (faceright && distance > 0 && distance < 0.65 && heightdis < 0.5)
            {
                Debug.Log("Attack!");
                attacking = true;

                anim.SetBool("walk", false);
                anim.SetBool("attack", true);
            
            }
            else if (!faceright && distance < 0 && distance > -0.65 && heightdis < 0.5)
            {
                Debug.Log("Attack!");
                attacking = true;

                anim.SetBool("walk", false);
                anim.SetBool("attack", true);
                
            }
          
        }
    }

    public void finishAttack()
    {
        anim.SetBool("attack", false);
        attacking = false;
    }
}
       