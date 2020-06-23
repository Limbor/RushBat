using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DwarfMovement : EnemyMovement
{
    private bool jump;
    private bool slash;
    private bool spin;
    private bool hack;

    private int attackType; //1普攻，2劈，3旋转
    private float spinTime;
    private float SpinTime;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        jump = false;
        slash = false;
        spin = false;
        hack = false;
        faceright = true;

        attackType = 1;
        maxBlood = 250;
        blood = maxBlood;
        walkspeed = 100f;
        SpinTime = 3f;
        spinTime = SpinTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isdead) return;
        
        Alive();
    }

    void FixedUpdate()
    {
        if (isdead) return;
        Move();
    }

    void Move()
    {
        //和玩家的距离
        Vector2 distance = transform.position - player.transform.position;
        Vector2 abs_dis = new Vector2(Mathf.Abs(distance.x), Mathf.Abs(distance.y));

        if (attacking)
        {
            //rb.velocity = new Vector2(0, rb.velocity.y);
            if (spin)
            {
                int direction = distance.x < 0 ? 1 : -1;
                rb.velocity = new Vector2(direction * 0.6f * walkspeed * Time.deltaTime, rb.velocity.y);
            }
            return;
        }

        //控制转身
        if ((faceright && distance.x > 0) || (!faceright && distance.x < 0)) 
        {
            Flip();
        }
        if (abs_dis.x < 0.7 && abs_dis.y < 0.5)
        {
            anim.SetBool("run", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
            Attack();
        }
        else
        {
            anim.SetBool("run", true);
            int direction = faceright ? 1 : -1;
            rb.velocity = new Vector2(direction * walkspeed * Time.deltaTime, rb.velocity.y);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 0.7f, groundLayer.value);
            if (hit)
            {
                Jump();
            }
        }
       
    }

    void Jump()
    {
        if (ground)
        {
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            anim.SetBool("jump", true);
            ground = false;
        }
        
    }

    void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            switch (attackType)
            {
                case 1:
                    anim.SetBool("slash", true);
                    //slash = true;
                    attackType = 2;
                    Invoke("finishAttack", 1.5f);
                    break;
                case 2:
                    anim.SetBool("hack", true);
                    //hack = true;
                    attackType = 3;
                    Invoke("finishAttack", 1.5f);
                    break;
                case 3:
                    anim.SetBool("spin", true);
                    spin = true;
                    attackType = 1;
                    int direction = faceright ? 1 : -1;
                    //rb.velocity = new Vector2(10f, rb.velocity.y);
                    Debug.Log(rb.velocity);
                    Invoke("finishSpin", 3f);
                    //StartCoroutine(spinAttack(direction));
                    break;
            }
        }
        
    }

    void finishAttack()
    {
        attacking = false;
        //slash = false;
        //hack = false;
    }

    IEnumerator spinAttack(int direction)
    {
        if (spinTime > 0)
        {
            rb.velocity = new Vector2(direction * walkspeed * Time.deltaTime, rb.velocity.y);
            Debug.Log(rb.velocity);
            spinTime -= Time.deltaTime;
            yield return null;
        }
        else
        {
            spinTime = SpinTime;
            finishSpin();
        }
        yield return null;
    }
    void finishSpin()
    {
        attacking = false;
        spin = false;
        anim.SetBool("spin", false);
        
    }

    void finishAttackAnimation()
    {
        anim.SetBool("slash", false);
        anim.SetBool("hack", false);
    }

    //protected override void Alive()
    //{
    //    if (blood <= 0)
    //    {
    //        rb.velocity=new
    //    }
    //}

    public override void getDamage(float damage, int direction)
    {
        if (!isdead && !spin)
        {
            blood -= damage;
            bloodVolume.GetComponent<Image>().fillAmount = blood / maxBlood;
            finishAttackAnimation();
            anim.SetBool("hurt", true);
            //hurt = true;
            bloodtime = 2;

            if (hurttime <= 0)
            {
                hurttime = 0.1f;
            }
            float backDis = direction * 0.2f;
            //怪物受伤后退
            transform.position = new Vector2(transform.position.x + backDis, transform.position.y);
            PoolManager.GetInstance().GetDamageText(transform.position, damage);
        }
    }
}
