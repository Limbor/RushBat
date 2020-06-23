using System;
using UnityEngine;

public class DamageHero : MonoBehaviour
{
    public int damage;
    public bool poison;
    public bool burn;
    public int poisonTime;
    public int burnTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            float direction = other.transform.position.y - transform.position.y > 0 ? 1 : -1;
            if(poison) other.GetComponent<PlayerProperty>().GetPoisoned(poisonTime);
            if(burn) other.GetComponent<PlayerProperty>().GetBurnt(burnTime);
            other.GetComponent<PlayerMovement>().Hurt(damage, direction * Vector2.right, GameManager.Enemy);
        }
    }
}
