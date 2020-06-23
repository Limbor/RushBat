using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float damageScope;
    public LayerMask playerLayer;
    public LayerMask groundLayer;        
    public float flySpeed;
    

    private CircleCollider2D coll;
    private GameObject player;
    private Animator animator;
    private float direction;
    private bool collide;
    
    private void OnEnable()
    {
        player = GameManager.GetInstance().GetPlayer();
        direction = transform.localScale.x;
        transform.localScale = new Vector3(direction, 1, 1);

        coll = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        collide = false;
    }

    private void Update()
    {
        if (collide) return;
        transform.position = new Vector2(transform.position.x + flySpeed * Time.deltaTime * direction, transform.position.y);
        if (Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer) != null)
        {
            animator.SetTrigger("collide");
            collide = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {      
            player.GetComponent<PlayerMovement>().Hurt(damage, new Vector2(transform.localScale.x, 0), GameManager.Enemy); 
        }
         Disappear();
    }


     public void Disappear(){
          PoolManager.GetInstance().ReturnBulletPool(gameObject);
     }

}
