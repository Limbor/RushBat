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
    protected SpriteRenderer render;
    //怪物数据
    protected float walkspeed;
    protected float waittime;
    protected float blood;
    protected float maxBlood;
    protected float damage;
    protected float length;

    //怪物状态
    protected bool faceright;
    //protected bool jumping;
    protected bool isdead;
    protected bool attacking;
    protected bool ground;
    protected bool hurt;
    protected bool block;
    protected bool shield;
    protected bool canHurt;
    //画布和血条,血量,显示时间
    public GameObject canvas;
    protected Transform bloodGroove;
    protected Transform bloodVolume;
    protected float bloodtime;
    protected float hurttime;   //受伤会变红，并暂停动画0.5秒
    //玩家
    protected GameObject player;
    protected LayerMask groundLayer;
    protected LayerMask platformLayer;

    //掉落物
    public GameObject heartPrefab;
    public GameObject goldPrefab;
    public GameObject silverPrefab;

    // Start is called before the first frame update
    protected void Start()
    {
        GetComponentInParent<Room>().RegisterEnemy(gameObject);
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        player = GameManager.GetInstance().GetPlayer();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        platformLayer = 1 << LayerMask.NameToLayer("Platform");
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
        hurt = false;
        ground = false;
        block = false;
        shield = false;
        canHurt = true;

        //transform.position = startPos.position;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (isdead) return;
        Alive();
    }

    // 当接触到地面时，设置跳跃条件为假
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Platform") ||
            coll.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Debug.Log("OnGround");
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

    //判断是否存活，并显示血条, 受伤时会变白闪烁
    protected virtual void Alive()
    {
        //Debug.Log(blood);
        if (blood <= 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            //死亡后血条瞬间消失
            isdead = true;
            anim.SetBool("dead", true);
            canvas.SetActive(false);

            //掉落物品
            Drop();

            GetComponentInParent<Room>().DelEnemy(gameObject);
            GetComponent<SpriteRenderer>().color = Color.gray;
            GetComponent<SpriteRenderer>().sortingLayerName = "Environment";
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            render.material.SetFloat("_FlashAmount", 0);
            hurt = false;
            Destroy(transform.GetComponent<Rigidbody2D>());
            Destroy(transform.GetComponent<Collider2D>());
        }
        else
        {
            //血条逐渐消失
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

    public bool Direction()
    {
        return faceright;
    }

    //怪物受伤，指定伤害和后退方向，血条改变
    public virtual void getDamage(float damage, int direction)
    {

        if (!isdead && canHurt) 
        {
            blood -= damage;
            bloodVolume.GetComponent<Image>().fillAmount = blood / maxBlood;
            // Debug.Log("Current health: " + blood);

            //受伤停止攻击
            //anim.SetBool("attack", false);
            //attacking = false;
            anim.SetBool("attack", false);
            attacking = false;
            anim.SetBool("shield", false);
            shield = false;
            anim.SetBool("hurt", true);
            hurt = true;
            //transform.GetComponent<SpriteRenderer>().color;

            bloodtime = 2;         //血条显示时间

            if (hurttime <= 0)
            {
                //怪物受伤闪烁时间，
                hurttime = 0.1f;
            }

            float backDis = direction * 0.2f;
            //怪物受伤后退
            transform.position = new Vector2(transform.position.x + backDis, transform.position.y);
        }
        
    }

    protected RaycastHit2D Raycast(Vector2 offset, Vector2 direction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, direction, length, layer);
        return hit;
    }

    protected virtual void Drop()
    {

    }

    public bool canGetDamage()
    {
        return canHurt;
    }

    public bool IsDead()
    {
        return isdead;
    }
}
