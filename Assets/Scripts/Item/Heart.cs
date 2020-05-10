using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    public int value;
    protected override void Start()
    {
        base.Start();
        pickCondition = true;
    }

    protected override void Effect()
    {
        player.GetComponent<PlayerProperty>().SetHealth(value);
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
        if (!player.IsHealthy())
        {
            pickCondition = false;
        }
    }
}
