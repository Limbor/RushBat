using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOneTime : MonoBehaviour
{
    public bool destroy;
    public bool auto;
    public float time;

    private void Start()
    {
        if (auto)
        {
            Destroy(gameObject, time);
        }
    }

    public void Disappear()
    {
        if (destroy) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}
