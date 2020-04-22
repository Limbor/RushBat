using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOneTime : MonoBehaviour
{
    public bool destroy;

    public void Disappear()
    {
        if (destroy) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}
