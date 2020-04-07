using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Stab());
    }

    IEnumerator Stab()
    {
        yield return new WaitForSeconds(3);
        anim.SetTrigger("spike");
        StartCoroutine(Stab());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 direction = (collision.transform.position - transform.position + new Vector3(0, 0.5f, 0)).normalized;
            collision.GetComponent<PlayerMovement>().Hurt(1, direction * 5f);
        }
    }
}
