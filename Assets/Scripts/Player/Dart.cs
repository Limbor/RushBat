using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    private float direction;
    private bool back;
    private bool isReturning;

    public LayerMask groundLayer;
    public float damage, floatRange;
    
    private float flySpeed = 10f;
    private float slowdown = -8f;
    private List<GameObject> attackedEnemies = new List<GameObject>();
    private PlayerAttack attack;

    private void OnEnable()
    {
        GameObject player = GameManager.GetInstance().GetPlayer();
        attack = player.GetComponent<PlayerAttack>();
        back = player.GetComponent<PlayerProperty>().HaveSkill("Boomerang");

        flySpeed = 10f;
        isReturning = false;
        direction = player.transform.localScale.x;
        transform.localScale = new Vector3(direction, 1, 1);
        attackedEnemies.Clear();
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + flySpeed * Time.deltaTime * direction, transform.position.y);
        if (Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer) != null)
        {
            PoolManager.GetInstance().ReturnDartPool(gameObject);
        }

        if (back)
        {
            flySpeed += slowdown * Time.deltaTime;
            if (flySpeed < 0 && !isReturning)
            {
                isReturning = true;
                attackedEnemies.Clear();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (attackedEnemies.Contains(collision.gameObject)) return;
            attackedEnemies.Add(collision.gameObject);
            float damage = this.damage + Random.Range(-floatRange, floatRange);
            attack.Damage(collision.gameObject, damage, isReturning ? -direction : direction);
        }
        else if (isReturning && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PoolManager.GetInstance().ReturnDartPool(gameObject);
        }
    }
}
