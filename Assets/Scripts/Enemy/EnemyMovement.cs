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
    //画布和血条
    public GameObject canvas;
    protected Transform bloodGroove;
    //玩家
    public GameObject player;
    

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        bloodGroove = transform.Find("Canvas/BloodGroove/Blood");
        bloodGroove.GetComponent<Image>().fillAmount=1;
        

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

    //判断是否存活
    void Alive()
    {
        if (blood <= 0)
        {
            isdead = true;
            anim.SetBool("dead", true);
            canvas.SetActive(false);
            
        }
    }

    public bool IsDead()
    {
        return isdead;
    }

    public void GetDamage(float damage)
    {
        if (!isdead)
        {
            blood -= damage;
            bloodGroove.GetComponent<Image>().fillAmount = blood / 100;
            Debug.Log("Current health: " + blood);

            //transform.GetComponent<SpriteRenderer>().color;
        }
        
    }

}
