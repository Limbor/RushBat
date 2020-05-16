using UnityEngine;
using System;

public class ActionTimer : MonoBehaviour
{
    private Action timeOnAction;
    private float elapsedTime;

    public void SetTimer(float time, Action callBack)
    {
        elapsedTime = time;
        timeOnAction = callBack;
    }

    private void Update()
    {
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime <= 0)
            {
                timeOnAction();
                Destroy(this);
            }
        }
    }
}
