using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public int number;
    protected override void Effect()
    {
        player.SetCoinNumber(number);
    }

    protected override void OnGround()
    {
        base.OnGround();
        if (player.HaveEquipment("CoinBomb"))
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        Instantiate(Resources.Load<GameObject>("Prefabs/FX/Bomb"), 
            transform.position, Quaternion.identity);
    }
}
