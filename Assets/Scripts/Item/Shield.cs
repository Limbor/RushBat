using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{
   protected override void Start()
   {
      base.Start();
      price = 10;
      if(isGoods) value.text = price.ToString();
   }

   protected override void Effect()
   {   
      player.SetShield(1);
   }
}
