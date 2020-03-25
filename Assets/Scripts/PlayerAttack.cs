using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform damagePoint;
    public Transform energyPoint;
    public Transform dartPoint;
    public LayerMask enemyLayer;
    public GameObject energy;
    public GameObject dart;

    private float scope = 0.2f;
    private PlayerMovement player;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(damagePoint.position, scope);
    //}

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(damagePoint.position, scope, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            // TODO
            Debug.Log("damage");
        }
    }

    public void Hadouken()
    {
        player.canMove = true;
        energy.transform.position = energyPoint.position;
        energy.SetActive(true);
    }

    public void Throw()
    {
        dart.transform.position = dartPoint.position;
        dart.SetActive(true);
    }
}
