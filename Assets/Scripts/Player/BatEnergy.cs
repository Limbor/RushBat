using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnergy : MonoBehaviour
{
    public float damage, floatRange;
    public float boost;
    
    private CircleCollider2D coll;
    private Animator animator;
    private float startTime;
    private float direction;
    private bool collide;
    private bool power;
    private List<GameObject> attackedEnemies = new List<GameObject>();

    public LayerMask groundLayer;
    public float flySpeed;
    public float changeTime;
    
    private void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        direction = player.transform.localScale.x;
        transform.localScale = new Vector3(direction, 1, 1);

        coll = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        startTime = Time.time;
        collide = false;
        power = false;
        attackedEnemies.Clear();
    }

    private void Update()
    {
        if (collide) return;
        transform.position = new Vector2(transform.position.x + flySpeed * Time.deltaTime * direction, transform.position.y);
        if(Time.time >= startTime + changeTime)
        {
            power = true;
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
            if (attackedEnemies.Contains(collision.gameObject)) return;
            attackedEnemies.Add(collision.gameObject);
            float damage = this.damage + (power ? boost : 0);
            collision.GetComponent<EnemyMovement>().getDamage(damage + Random.Range(-floatRange, floatRange), 
                (int)(transform.localScale.x));
        }
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }
}
