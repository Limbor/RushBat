using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{
   protected override void Effect()
   {   
      player.SetShield(1);
   }
}
