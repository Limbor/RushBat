using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    private float direction;

    public LayerMask groundLayer;
    public float damage, floatRange;
    
    private float flySpeed = 10f;
    private List<GameObject> attackedEnemies = new List<GameObject>();
    private PlayerProperty property;

    private void OnEnable()
    {
        GameObject player = GameManager.GetInstance().GetPlayer();
        property = player.GetComponent<PlayerProperty>();

        direction = player.transform.localScale.x;
        transform.localScale = new Vector3(direction, 1, 1);
        attackedEnemies.Clear();
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + flySpeed * Time.deltaTime * direction, transform.position.y);
        if (Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer) != null)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (attackedEnemies.Contains(collision.gameObject)) return;
            attackedEnemies.Add(collision.gameObject);
            int extraDamage = 0;
            if (property.HaveEquipment("ShadowBlade") && direction * collision.transform.localScale.x > 0)
                extraDamage += 10;
            collision.GetComponent<EnemyMovement>().getDamage(
                damage + Random.Range(-floatRange, floatRange) + extraDamage, (int)transform.localScale.x);
        }
    }
}
