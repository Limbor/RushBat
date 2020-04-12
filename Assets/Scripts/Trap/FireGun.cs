using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : MonoBehaviour
{
    public float lastFireTime = 0f;
    private float coolDown = 3f;

    private void Update()
    {
        if(Time.time > lastFireTime + coolDown)
        {
            PoolManager.GetInstance().GetFireObject(transform.position, transform.localScale.x);
            lastFireTime = Time.time;
        }
    }
}
