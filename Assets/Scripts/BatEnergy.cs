using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnergy : MonoBehaviour
{
    private CircleCollider2D coll;
    private Rigidbody2D rb;
    private Animator animator;
    private float startTime;
    private float direction;

    public LayerMask groundLayer;
    public float flySpeed;
    public bool powerful = false;
    public float changeTime;

    private void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        direction = player.transform.localScale.x;
        transform.localScale = new Vector3(direction, 1, 1);

        coll = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        startTime = Time.time;
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + flySpeed * Time.deltaTime * direction, transform.position.y);
        if(Time.time >= startTime + changeTime)
        {
            animator.SetTrigger("change");
            coll.radius = 0.21f;
        }
        if (Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer) != null)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("damage");
        }
    }
}
