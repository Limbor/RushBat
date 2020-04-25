using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dizzy : MonoBehaviour
{
    private float exitTime;

    public void SetExitTime(float time)
    {
        exitTime = time;
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(exitTime);
        Destroy(gameObject);
    }
}
