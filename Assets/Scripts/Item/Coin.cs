using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public int value;
    protected override void Effect()
    {
        player.SetCoinNumber(value);
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
