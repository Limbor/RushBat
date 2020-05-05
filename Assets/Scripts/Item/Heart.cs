using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    private GameObject player;
    protected override void Start()
    {
        base.Start();
        player = GameManager.GetInstance().GetPlayer();
    }

    protected override void Effect()
    {
        player.GetComponent<PlayerProperty>().SetHealth(4);
    }
}
