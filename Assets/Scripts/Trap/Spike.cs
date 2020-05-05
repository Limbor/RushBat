using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private Animator anim;
    private float coolDown = 3f;

    public float lastStabTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Time.time > lastStabTime + coolDown)
        {
            anim.SetTrigger("spike");
            lastStabTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 direction = (collision.transform.position - transform.position + new Vector3(0, 0.5f, 0)).normalized;
            collision.GetComponent<PlayerMovement>().Hurt(1, direction * 5f, GameManager.GroundTrap);
        }
    }
}
