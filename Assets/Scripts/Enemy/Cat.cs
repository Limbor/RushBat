using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public bool isBoss;

    public GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider2D>().enabled = isBoss;
    }

    private void Update()
    {
        boss.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        GetComponent<Animator>().SetTrigger("transform");
    }

    private void Transform()
    {
        boss.SetActive(true);
        boss.transform.position = transform.position;
        gameObject.SetActive(false);
    }
}
