using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnergy : MonoBehaviour
{
    private CircleCollider2D coll;
    private Animator animator;
    private float startTime;
    private float direction;
    private bool collide;

    public LayerMask groundLayer;
    public float flySpeed;
    public bool powerful;
    public float changeTime;

    private void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        direction = player.transform.localScale.x;
        transform.localScale = new Vector3(direction, 1, 1);

        coll = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        startTime = Time.time;
        powerful = false;
        collide = false;
    }

    private void Update()
    {
        if (collide) return;
        transform.position = new Vector2(transform.position.x + flySpeed * Time.deltaTime * direction, transform.position.y);
        if(Time.time >= startTime + changeTime)
        {
            powerful = true;
            animator.SetTrigger("change");
            coll.radius = 0.21f;
        }
        if (Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer) != null)
        {
            animator.SetTrigger("collide");
            collide = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("damage");
        }
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }
}
