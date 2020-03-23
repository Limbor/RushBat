using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform damagePoint;
    public LayerMask enemyLayer;

    private float scope = 0.2f;

    public void Damage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(damagePoint.position, scope, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            // TODO
            Debug.Log("damage");
        }
    }
}
