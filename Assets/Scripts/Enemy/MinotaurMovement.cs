using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurMovement : EnemyMovement
{
    private bool run;
    private bool dash;
    private bool stab;
    private bool down;
    private bool rotate;
    private bool attackType;

    private float dashDistance;
    private float breathTime;
    private float weakTime;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //状态
        run = false;
        dash = false;
        stab = false;
        down = false;
        rotate = false;
        attackType = true;

        blood = 300f;
        walkspeed = 80f;
        faceright = true;
        dashDistance = 3f;
        breathTime = 1f;
        weakTime = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        Move();
        //Attack();
    }

    void Move()
    {
        if (isdead || stab || down || rotate)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
        if (dash)
        {
            return;
        }
        //和玩家的距离
        Vector2 distance = transform.position - player.transform.position;
        Vector2 abs_dis = new Vector2(Mathf.Abs(distance.x), Mathf.Abs(distance.y));
        if (distance.x >= 0)
        {
            //Debug.Log("distance>0!  faceright"+faceright);
            if (faceright)
            {
                //Debug.Log("faceright!");
                Flip();
            }
            if (abs_dis.x > dashDistance)
            {
                Debug.Log("Distance: " + abs_dis.x);
                StartCoroutine(Dash(-1));
            }
            else
            {
                anim.SetBool("run", true);
                rb.velocity = new Vector2(-1 * walkspeed * Time.deltaTime, rb.velocity.y);
                if (abs_dis.x <= 0.5f)
                {
                    Attack();
                }
            }
        }
        else
        {
            if (!faceright)
            {
                Flip();
            }
            if (abs_dis.x > dashDistance)
            {
                StartCoroutine(Dash(1));
            }
            else
            {
                anim.SetBool("run", true);
                rb.velocity = new Vector2(walkspeed * Time.deltaTime, rb.velocity.y);
                if (abs_dis.x <= 0.5f)
                {
                    Attack();
                }
            }
        }
    }

    IEnumerator Dash(int dir)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        dash = true;
        anim.SetBool("breath", true);
        anim.SetBool("run", false);
        yield return new WaitForSeconds(breathTime);
        anim.SetBool("breath", false);
        anim.SetBool("run", true);
        while (Mathf.Abs(transform.position.x - player.transform.position.x) >= 0.7f)
        {
            Debug.Log("Distance: " + Mathf.Abs(transform.position.x - player.transform.position.x));
            rb.velocity = new Vector2(dir * walkspeed * Time.deltaTime, rb.velocity.y);
            yield return null;
        }
        //anim.SetBool("run", false);
        rb.velocity = new Vector2(0, rb.velocity.y);
        Debug.Log("finishRun");
        anim.SetBool("chop", true);

    }

    void finishDash()
    {
        Debug.Log("finish Dash");
        anim.SetBool("run", false);
        anim.SetBool("chop", false);
        anim.SetBool("weak", true);
        Invoke("finishWeak", weakTime);
    }

    void finishWeak()
    {
        dash = false;
        anim.SetBool("weak", false);
    }

    void Attack()
    {
        attacking = true;
        if (attackType)
        {
            anim.SetBool("stab", true);
            stab = true;
            attackType = !attackType;
        }
        else
        {
            anim.SetBool("rotate", true);
            rotate = true;
            attackType = !attackType;
        }
    }

    void addBlood()
    {
        blood += 50;
        if (blood >= 300)
        {
            blood = 300;
        }
    }

    void finishAttack()
    {
        anim.SetBool("stab", false);
        stab = false;
        anim.SetBool("rotate", false);
        rotate = false;
    }
}
