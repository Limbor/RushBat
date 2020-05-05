using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject pick;

    protected void Awake()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), 
            GameManager.GetInstance().GetPlayer().GetComponent<Collider2D>(), true);
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        pick = Resources.Load<GameObject>("Prefabs/Item/Pick");
        StartCoroutine(CanPick());
    }

    IEnumerator CanPick()
    {
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), 
            GameManager.GetInstance().GetPlayer().GetComponent<Collider2D>(), false);
    }
    
    protected virtual void Effect()
    {
        
    }
    
    private void Pick()
    {
        Effect();
        Destroy(gameObject);
        Instantiate(pick, transform.position, Quaternion.identity);
    }
    
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), 
            GameManager.GetInstance().GetPlayer().GetComponent<Collider2D>(), false);
    }

    protected void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Pick();
        }
    }
}

