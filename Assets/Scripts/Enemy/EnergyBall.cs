using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public int damage;
    public float damageScope;
    public LayerMask playerLayer;
    public LayerMask groundLayer;        
    public float flySpeed;
    public float changeTime;

    private CircleCollider2D coll;
    private GameObject player;
    private Animator animator;
    private float startTime;
    private float direction;
    private bool collide;
    private EnergyBallAttack attack;
    
    private void OnEnable()
    {
        player = GameManager.GetInstance().GetPlayer();
        direction = transform.localScale.x;
        transform.localScale = new Vector3(direction, 1, 1);

        coll = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        startTime = Time.time;
        collide = false;
    }

    private void Update()
    {
        if (collide) return;
        transform.position = new Vector2(transform.position.x + flySpeed * Time.deltaTime * direction, transform.position.y);
        if(Time.time >= startTime + changeTime)
        {
            animator.SetTrigger("fly");
        }
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
            player.GetComponent<PlayerProperty>().GetBurnt(2);
            player.GetComponent<PlayerMovement>().Hurt(damage, new Vector2(transform.localScale.x, 0), GameManager.Enemy); 
        }
         Disappear();
    }


     public void Disappear(){
          PoolManager.GetInstance().ReturnEnergyBallPool(gameObject);
     }

}
