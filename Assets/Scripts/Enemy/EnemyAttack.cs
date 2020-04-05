using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public LayerMask playerLayer;
    public Transform damagePoint;

    protected float normalDamage;
    protected float normalScope;

    protected void normalAttack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(damagePoint.position, normalScope, playerLayer);
        foreach (Collider2D player in players)
        {
            // TODO
            
            Debug.Log("Enemy take damage, Amount: "+players.Length);
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<AxeMovement>().getDamage(30);
            break;
        }
    }

}
