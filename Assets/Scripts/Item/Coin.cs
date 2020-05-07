﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public int value;
    protected override void Effect()
    {
        player.SetCoinNumber(value);
    }
}
