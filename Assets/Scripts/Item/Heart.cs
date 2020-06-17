using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    public int heal;
    protected override void Start()
    {
        base.Start();
        pickCondition = true;
        price = heal * 5 + 5;
        if(isGoods) value.text = price.ToString();
    }

    protected override void Effect()
    {
        player.GetComponent<PlayerProperty>().SetHealth(heal);
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
