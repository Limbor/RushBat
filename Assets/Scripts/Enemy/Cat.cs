using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public bool isBoss; 
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider2D>().enabled = isBoss;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        GetComponent<Animator>().SetTrigger("transform");
    }

    private void Transform()
    {
        var boss = (GameObject)Instantiate(Resources.Load("Prefabs/Enemy/Demon"), transform.parent);
        boss.transform.position = transform.position;
        Destroy(gameObject);
    }
}
