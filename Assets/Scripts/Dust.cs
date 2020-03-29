using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public void Disappear()
    {
        PoolManager.GetInstance().ReturnDustPool(gameObject);
    }
}
