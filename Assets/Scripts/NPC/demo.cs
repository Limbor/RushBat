using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour
{

    public float speed =5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 position  = transform.position;
        position.x+=moveX*speed*Time.deltaTime;
        position.y+=moveY*speed*Time.deltaTime;
        transform.position = position; 
    }
}
