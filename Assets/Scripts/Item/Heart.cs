using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    protected override void Effect()
    {
        player.GetComponent<PlayerProperty>().SetHealth(4);
    }
}
