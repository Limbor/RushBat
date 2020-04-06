using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    //要继承的属性
    //组件
    protected Rigidbody2D rb; 
    protected Animator anim;
    //怪物数据
    protected float walkspeed;
    protected float waittime;
    protected float blood;
    protected float damage;

    //怪物状态
    protected bool faceright;
    //protected bool jumping;
    protected bool isdead;
    protected bool attacking;
    protected bool ground;
    //画布和血条,血量,显示时间
    public GameObject canvas;
    protected Transform bloodGroove;
    protected Transform bloodVolume;
    protected float bloodtime;
    protected float hurttime;   //受伤会变红，并暂停动画0.5秒
    //玩家
    protected GameObject player;
    

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        bloodGroove = transform.Find("Canvas/BloodGroove");
        bloodVolume = transform.Find("Canvas/BloodGroove/Blood");
        bloodVolume.GetComponent<Image>().fillAmount=1;
        bloodtime = 0;
        hurttime = 0;
        bloodGroove.GetComponent<Image>().color = new Color(
            bloodGroove.GetComponent<Image>().color.r,
            bloodGroove.GetComponent<Image>().color.g,
            bloodGroove.GetComponent<Image>().color.b,
            0);
        bloodVolume.GetComponent<Image>().color = new Color(
            bloodVolume.GetComponent<Image>().color.r,
            bloodVolume.GetComponent<Image>().color.g,
            bloodVolume.GetComponent<Image>().color.b,
            0);


        //jumping = false;
        isdead = false;
        attacking = false;
        ground = false;

        //transform.position = startPos.position;
    }

    // Update is called once per frame
    protected void Update()
    {
        Alive();
    }

    // 当接触到地面时，设置跳跃条件为假
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 9)
        {
            //jumping = false;
            Debug.Log("On Ground!");
            ground = true;
            anim.SetBool("jump", false);
        }
    }

    //转向
    protected void Flip()
    {
        faceright = !faceright;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        Vector3 bloodScale = transform.Find("Canvas/BloodGroove").localScale;
        bloodScale.x *= -1;
        transform.Find("Canvas/BloodGroove").localScale = bloodScale;
    }

    //判断是否存活，并显示血条, 受伤时会变红，并暂停动画0.5s
    void Alive()
    {
        if (blood <= 0)
        {
            isdead = true;
            anim.SetBool("dead", true);
            canvas.SetActive(false);
            
        }
        else
        {
            bloodtime -= Time.deltaTime;
            bloodGroove.GetComponent<Image>().color = new Color(
                bloodGroove.GetComponent<Image>().color.r,
                bloodGroove.GetComponent<Image>().color.g,
                bloodGroove.GetComponent<Image>().color.b,
                bloodtime / 2);
                
            bloodVolume.GetComponent<Image>().color = new Color(
                bloodVolume.GetComponent<Image>().color.r,
                bloodVolume.GetComponent<Image>().color.g,
                bloodVolume.GetComponent<Image>().color.b,
                bloodtime / 2);

            hurttime -= Time.deltaTime;
            if (hurttime > 0)
            {
                transform.GetComponent<SpriteRenderer>().color = Color.red;
                anim.speed = 0;
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().color = Color.white;
                anim.speed = 1;
            }

        }
        
    }

    public bool IsDead()
    {
        return isdead;
    }

    public void getDamage(float damage)
    {
        if (!isdead)
        {
            blood -= damage;
            bloodVolume.GetComponent<Image>().fillAmount = blood / 100;
            Debug.Log("Current health: " + blood);

            //transform.GetComponent<SpriteRenderer>().color;
            
            bloodtime = 2;

            if (hurttime <= 0)
            {
                hurttime = 0.5f;
            }

        }
        
    }


}
