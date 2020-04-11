using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidTear : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D coll;
    private float speed;
    private bool splash;

    public LayerMask groundLayer;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<CircleCollider2D>();
        speed = 0f;
        splash = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (splash) return;
        speed += 30f * Time.deltaTime;
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (Physics2D.OverlapCircle(transform.position + new Vector3(coll.offset.x, coll.offset.y, 0f), 0.05f, groundLayer) != null)
        {
            anim.SetTrigger("splash");
            splash = true;
        }
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
