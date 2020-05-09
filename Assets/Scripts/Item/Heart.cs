using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    public int value;
    
    protected override void Effect()
    {
        player.GetComponent<PlayerProperty>().SetHealth(value);
    }
}
