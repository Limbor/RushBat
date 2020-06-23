using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpike : MonoBehaviour
{
    [SerializeField]
    private float speed = 8f;

    private float liftTime;

    private float direction;

    private void OnEnable()
    {
        direction = transform.localScale.x;
        liftTime = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        liftTime -= Time.deltaTime;
        if (liftTime <= 0)
        {
            PoolManager.GetInstance().ReturnSpikePool(gameObject);
        }
        transform.Translate( direction * speed * Time.deltaTime * Vector3.right);
        if(Physics2D.Raycast(transform.position, Vector2.right * direction, 0.2f, 1 << LayerMask.NameToLayer("Ground")))
        {
            PoolManager.GetInstance().ReturnSpikePool(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            collision.GetComponent<PlayerMovement>().Hurt(1, direction * 2f, GameManager.FlyingTrap);
        }
    }
}
