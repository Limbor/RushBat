using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float lastTime = -10f;
    private float coolDown = 2f;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= lastTime + coolDown)
        {
            lastTime = Time.time;
            anim.SetTrigger("rotate");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 direction = collision.transform.position - transform.position;
            collision.GetComponent<PlayerMovement>().Hurt(1, direction.normalized * 3f);
        }
    }
}
