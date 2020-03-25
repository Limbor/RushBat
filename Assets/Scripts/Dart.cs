using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    private float direction;

    public LayerMask groundLayer;
    public float flySpeed;

    private void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        direction = player.transform.localScale.x;
        transform.localScale = new Vector3(direction, 1, 1);
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
            Debug.Log("damage");
        }
    }
}
