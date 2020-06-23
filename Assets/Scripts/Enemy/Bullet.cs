using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
     public int damage;
    public float damageScope;
    private CircleCollider2D coll;
    private Animator animator;
    private float startTime;
    private float direction;
    private bool collide;
    private List<GameObject> attackedPlayers = new List<GameObject>();
    private GunAttack attack;
    
    public LayerMask groundLayer;
    public float flySpeed;
    public float changeTime;

    private GameObject imp;
    
    private void OnEnable()
    {
        // imp =GameObject.Find("Imp");
        // attack = imp.GetComponent<GunAttack>();
        //
        // direction = imp.transform.localScale.x;
        // transform.localScale = new Vector3(direction, 1, 1);

        coll = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        startTime = Time.time;
        collide = false;
        attackedPlayers.Clear();
    }

    private void Update()
    {
        if (collide) return;
        transform.position = new Vector2(transform.position.x + flySpeed * Time.deltaTime * direction, transform.position.y);
        if (Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer) != null)
        {
            PoolManager.GetInstance().ReturnBulletPool(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (attackedPlayers.Contains(collision.gameObject)) return;
            attackedPlayers.Add(collision.gameObject);
            int damage = this.damage ;
            attack.gunAttack(transform.position,damage, damageScope);
        }
        else {
            PoolManager.GetInstance().ReturnBulletPool(gameObject);
        }
    }
}
