using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public float speed;
    public float xOffset, yOffset;

    private Transform target;
    private Vector3 position;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        position = target.position + offset;
    }


    private void Update()
    {
        if (!(Mathf.Abs((target.position + offset).x - transform.position.x) <= xOffset &&
            Mathf.Abs((target.position + offset).y - transform.position.y) <= yOffset))
        {
            position = target.position + offset;
        }
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }

    public void Shake()
    {
        transform.DOComplete();
        transform.DOShakePosition(0.2f, 0.5f, 14, 90, false,true);
    }
}
