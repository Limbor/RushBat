using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidTear : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D coll;
    private float speed;
    private bool splash;
    private float destination;

    public LayerMask groundLayer;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<CircleCollider2D>();
        speed = 0f;
        splash = false;
        RaycastHit2D checkGround = Physics2D.Raycast(transform.position + new Vector3(coll.offset.x, coll.offset.y, 0), Vector2.down,
            float.PositiveInfinity, 1 << LayerMask.NameToLayer("Ground"));
        destination = transform.position.y - checkGround.distance;
    }

    // Update is called once per frame
    void Update()
    {
        if (splash) return;
        speed += 10f * Time.deltaTime;
        transform.position += Vector3.down * speed * Time.deltaTime;
        if(transform.position.y <= destination + coll.radius)
        {
            anim.SetTrigger("splash");
            splash = true;
        }
    }

    private void FixedUpdate()
    {
        //if (splash) return;
        //if (Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer) != null)
        //{
        //    anim.SetTrigger("splash");
        //    splash = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerProperty>().GetPoisoned(3);
            anim.SetTrigger("splash");
            splash = true;
        }
    }

    public void Disappear()
    {
        PoolManager.GetInstance().ReturnTearPool(gameObject);
    }
}
