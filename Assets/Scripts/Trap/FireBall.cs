using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;

    private float direction;
    private bool boom;

    private void OnEnable()
    {
        boom = false;
        direction = transform.localScale.y;
        transform.position += new Vector3(0.3f * direction, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (boom) return;
        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
    }

    private void FixedUpdate()
    {
        if (boom) return;
        if (Physics2D.OverlapCircle(transform.position, 0.1f, 1 << LayerMask.NameToLayer("Ground")) != null)
        {
            GetComponent<Animator>().SetTrigger("boom");
            boom = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !boom)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            collision.GetComponent<PlayerProperty>().GetBurnt(2);
            collision.GetComponent<PlayerMovement>().Hurt(1, direction * 2f, GameManager.FlyingTrap);
            GetComponent<Animator>().SetTrigger("boom");
            boom = true;
        }
    }

    public void Disappear()
    {
        PoolManager.GetInstance().ReturnFirePool(gameObject);
    }
}
