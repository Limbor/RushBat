using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float direction;

    public float speed;
    public LayerMask groundLayer;
    public int normalDamage = 3;
    public float normalScope = 0.1f;


    private void OnEnable()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("RangedEnemy");
        direction = enemy.transform.localScale.x/3;
        Debug.Log(direction);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime * direction, transform.position.y);
        if (Physics2D.OverlapCircle(transform.position, normalScope, groundLayer) != null)
        {
            gameObject.SetActive(false);
        }
    }

    // void Update()
    // {
    //     transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
         {
             collision.GetComponent<PlayerMovement>().Hurt(normalDamage, new Vector2(direction,0), GameManager.Enemy);
             Destroy(gameObject);
         }

    }

    // public void Disappear()
    // {
    //     PoolManager.GetInstance().ReturnBulletPool(gameObject);
    // }
}
