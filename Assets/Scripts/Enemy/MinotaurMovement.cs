using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinotaurMovement : EnemyMovement
{
    public GameObject dizzyPrefab;
    public GameObject healPrefab;
    private GameObject healIcon;

    public Transform bloodCanvas;
    private Transform blood1;
    private Transform blood2;
    private Transform blood3;
    private Transform blood4;
    private Transform blood5;


    private bool run;
    private bool dash;
    private bool stab;
    private bool down;
    private bool rotate;
    private bool attackType;
    private bool hasAttacked;  //一次动画只造成一次伤害
    private bool recover;

    //private float maxBlood;
    private float dashDistance;
    private float breathTime;
    private float weakTime;
    private float recoverTime;
    private float recoverInterval;
    private float recoverIntervalS;
    //private float recoverAmount;
    private float attackInterval;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        healIcon = null;
        //状态
        run = false;
        dash = false;
        stab = false;
        down = false;
        rotate = false;
        attackType = true;
        hasAttacked = false;
        recover = false;

        //初始化血条
        blood1 = bloodCanvas.Find("Board/Blood1");
        blood2 = bloodCanvas.Find("Board/Blood2");
        blood3 = bloodCanvas.Find("Board/Blood3");
        blood4 = bloodCanvas.Find("Board/Blood4");
        blood5 = bloodCanvas.Find("Board/Blood5");
        blood1.GetComponent<Image>().fillAmount = 1;
        blood2.GetComponent<Image>().fillAmount = 1;
        blood3.GetComponent<Image>().fillAmount = 1;
        blood4.GetComponent<Image>().fillAmount = 1;
        blood5.GetComponent<Image>().fillAmount = 1;

        //各种属性值
        maxBlood = 500f;
        blood = maxBlood;
        walkspeed = 100f;
        faceright = true;
        dashDistance = 3f;
        breathTime = 1f;
        weakTime = 2f;
        recoverIntervalS = 10f;
        recoverInterval = recoverIntervalS;
        recoverTime = 3f;
        attackInterval = 0f;
    }

    // Update is called once per frame
    new void Update()
    {
        if (isdead) return;
        //base.Update();
        //attackInterval -= Time.deltaTime;
        Alive();
        Move();
        //Attack();
        Recover();
        changeBlood();
    }
    void FixedUpdate()
    {
        if (isdead) return;
    }

    void Move()
    {
        //Debug.Log("Move??????");
        //Debug.Log("stab:" + stab + ",rotate:" + rotate + ",hurt:" + hurt);
        if (isdead)
        {
            return;
        }
        if (stab || down || rotate || recover) 
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
            //Debug.Log("Move??");
            //Debug.Log("distance>0!  faceright"+faceright);
            if (faceright)
            {
                //Debug.Log("faceright!");
                Flip();
            }
            if (abs_dis.x > dashDistance)
            {
                //Debug.Log("Distance: " + abs_dis.x);
                StartCoroutine(Dash(-1));
            }
            
            else
            {
                if (abs_dis.x <= 0.8)
                {
                    anim.SetBool("run", false);
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else
                {
                    anim.SetBool("run", true);
                    rb.velocity = new Vector2(-1 * walkspeed * Time.deltaTime, rb.velocity.y);
                }
                
                if (abs_dis.x <= 1.1f)
                {
                    Attack();
                }
            }
        }
        else
        {
            //Debug.Log("Move??");
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
                if (abs_dis.x <= 0.8)
                {
                    anim.SetBool("run", false);
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else
                {
                    anim.SetBool("run", true);
                    rb.velocity = new Vector2(walkspeed * Time.deltaTime, rb.velocity.y);
                }
                
                if (abs_dis.x <= 1.1f)
                {
                    Attack();
                }
            }
        }
    }

    IEnumerator Dash(int dir)
    {
        dash = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.SetBool("breath", true);
        anim.SetBool("run", false);
        yield return new WaitForSeconds(breathTime);
        anim.SetBool("breath", false);
        anim.SetBool("run", true);
        while (Mathf.Abs(transform.position.x - player.transform.position.x) >= 0.7f)
        {
            //Debug.Log("Distance: " + Mathf.Abs(transform.position.x - player.transform.position.x));
            rb.velocity = new Vector2(dir * 1.5f * walkspeed * Time.deltaTime, rb.velocity.y);
            yield return null;
        }
        //anim.SetBool("run", false);
        rb.velocity = new Vector2(0, rb.velocity.y);
        Debug.Log("finishRun");
        anim.SetBool("chop", true);

    }

    void finishDash()
    {
        hasAttacked = false;
        Debug.Log("finish Dash");
        anim.SetBool("run", false);
        anim.SetBool("chop", false);
        anim.SetBool("weak", true);

        int direction = 1;
        if (!faceright)
        {
            direction = -1;
        }

        GameObject dizzyIcon1 = Instantiate(dizzyPrefab, new Vector2(transform.position.x + direction * 0.05f, transform.position.y + 0.32f), 
            new Quaternion(0, 0, 0, 0)) as GameObject;
        dizzyIcon1.transform.parent = transform;
        dizzyIcon1.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        dizzyIcon1.GetComponent<Dizzy>().SetExitTime(weakTime);

        GameObject dizzyIcon2 = Instantiate(dizzyPrefab, new Vector2(transform.position.x + direction * 0.25f, transform.position.y + 0.32f),
            new Quaternion(0, 0, 0, 0)) as GameObject;
        dizzyIcon2.transform.parent = transform;
        dizzyIcon2.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        dizzyIcon2.GetComponent<Dizzy>().SetExitTime(weakTime);
        Invoke("finishWeak", weakTime);

    }

    void finishWeak()
    {
        dash = false;
        anim.SetBool("weak", false);
    }

    void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            if (attackType)
            {
                anim.SetBool("stab", true);
                anim.SetBool("run", false);
                stab = true;
                attackType = !attackType;
                Invoke("finishAttack", 2.5f);
            }
            else
            {
                anim.SetBool("rotate", true);
                anim.SetBool("run", false);
                rotate = true;
                attackType = !attackType;
                Invoke("finishAttack", 2.5f);
            }
        }
        
        //attackInterval -= Time.deltaTime;
        //Debug.Log("attackInterval: "+attackInterval);
        //if (attackInterval <= 0)
        //{
        
        //}
     
    }

    private void changeBlood()
    {
        blood5.GetComponent<Image>().fillAmount = blood > 400 ? ((blood - 400) / 100) : 0;
        blood4.GetComponent<Image>().fillAmount = blood > 300 ? ((blood - 300) / 100) : 0;
        blood3.GetComponent<Image>().fillAmount = blood > 200 ? ((blood - 200) / 100) : 0;
        blood2.GetComponent<Image>().fillAmount = blood > 100 ? ((blood - 100) / 100) : 0;
        blood1.GetComponent<Image>().fillAmount = blood > 0 ? (blood / 100) : 0;

    }

    protected override void Alive()
    {
        if (blood <= 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
           
            anim.SetBool("dead", true);
            //canvas.SetActive(false);
            GameManager.GetInstance().DelEnemy(gameObject);
            GetComponent<SpriteRenderer>().color = Color.gray;
            GetComponent<SpriteRenderer>().sortingLayerName = "Environment";
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            Destroy(transform.GetComponent<Rigidbody2D>());
            Destroy(transform.GetComponent<CapsuleCollider2D>());
            isdead = true;
        }
        else
        {
            
            hurttime -= Time.deltaTime;
            if (hurttime > 0)
            {
                render.material.SetFloat("_FlashAmount", 1);
                //transform.GetComponent<SpriteRenderer>().color = Color.red;
                //anim.speed = 0;
            }
            else
            {
                render.material.SetFloat("_FlashAmount", 0);
                //transform.GetComponent<SpriteRenderer>().color = Color.white;
                //anim.speed = 1;
                anim.SetBool("hurt", false);
                //Debug.Log("finishHurt!");
                hurt = false;
            }

        }

    }

    //怪物受伤，指定伤害和后退方向，血条改变
    public override void getDamage(float damage, int direction)
    {
        if (!isdead)
        {
            blood -= damage;
            //bloodVolume.GetComponent<Image>().fillAmount = blood / 300;
            bloodVolume.GetComponent<Image>().fillAmount = blood / maxBlood;

            //受伤停止攻击
            finishAttackAnimation();
            finishRecover();
            //Debug.Log("getDamage!");
            attacking = false;
            anim.SetBool("hurt", true);
            hurt = true;
            //transform.GetComponent<SpriteRenderer>().color;

            if (hurttime <= 0)
            {
                //怪物受伤闪烁时间，
                hurttime = 0.1f;
            }

            float backDis = direction * 0.2f;
            //怪物受伤后退
            transform.position = new Vector2(transform.position.x + backDis, transform.position.y);

            PoolManager.GetInstance().GetDamageText(transform.position, damage);
        }

    }

    void Recover()
    {
        recoverInterval -= Time.deltaTime;
        if (blood > maxBlood)
        {
            blood = maxBlood;
        }
        if (recoverInterval <= 0)
        {
            recover = true;
            if (isdead)
            {
                finishRecover();
            }
            else if (!dash && !attacking) 
            {
                if (healIcon == null)
                {
                    healIcon = Instantiate(healPrefab, new Vector2(transform.position.x, transform.position.y + 0.8f),
                        new Quaternion(0, 0, 0, 0)) as GameObject;
                    healIcon.transform.parent = transform;
                }
                anim.SetBool("run", false);
                anim.SetBool("down", true);
                recoverTime -= Time.deltaTime;

                blood += 20 * Time.deltaTime;
                //bloodVolume.GetComponent<Image>().fillAmount = blood / 300;
                //Debug.Log("show!");
                //bloodGroove.GetComponent<Image>().color = new Color(
                //    bloodGroove.GetComponent<Image>().color.r,
                //    bloodGroove.GetComponent<Image>().color.g,
                //    bloodGroove.GetComponent<Image>().color.b,
                //    recoverTime/3);

                //bloodVolume.GetComponent<Image>().color = new Color(
                //    bloodVolume.GetComponent<Image>().color.r,
                //    bloodVolume.GetComponent<Image>().color.g,
                //    bloodVolume.GetComponent<Image>().color.b,
                //    recoverTime/3);
                if (recoverTime <= 0)
                {
                    finishRecover();
                }
            }
        }
    }

    void finishRecover()
    {
        Debug.Log("finishRecover!");
        Destroy(healIcon);
        //bloodGroove.GetComponent<Image>().color = new Color(
        //    bloodGroove.GetComponent<Image>().color.r,
        //    bloodGroove.GetComponent<Image>().color.g,
        //    bloodGroove.GetComponent<Image>().color.b,
        //    0);

        //bloodVolume.GetComponent<Image>().color = new Color(
        //    bloodVolume.GetComponent<Image>().color.r,
        //    bloodVolume.GetComponent<Image>().color.g,
        //    bloodVolume.GetComponent<Image>().color.b,
        //    0);

        recover = false;
        recoverInterval = recoverIntervalS;
        anim.SetBool("down", false);
        recoverTime = 3f;
    }
    
    void finishAttack()
    {
        attacking = false;
        Debug.Log("finishAttack!");
        //hasAttacked = false;
        //anim.SetBool("stab", false);
        stab = false;
        //anim.SetBool("rotate", false);
        rotate = false;
        //attackInterval = 3f;
    }
    void finishAttackAnimation()
    {
        anim.SetBool("stab", false);
        anim.SetBool("rotate", false);
    }
}
