using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform damagePoint;
    public LayerMask enemyLayer;

    private float scope = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, scope, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            // TODO
            enemy.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
