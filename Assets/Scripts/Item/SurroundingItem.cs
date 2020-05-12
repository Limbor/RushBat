using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SurroundingItem : MonoBehaviour
{
    protected Transform target;
    protected float radius = 0.6f;
    protected float rotateSpeed = 720f;
    protected float surroundSpeed = 180f;
    
    private Vector3 offset;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        offset = Vector3.up * radius;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position = target.position + offset;
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime, Space.Self);
        transform.RotateAround(target.position, Vector3.back, surroundSpeed * Time.deltaTime);
        offset = transform.position - target.position;
    }
}
