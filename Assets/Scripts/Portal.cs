using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Portal : MonoBehaviour
{
    private GameObject player;
    private bool begin = false;

    private void Start()
    {
        player = GameManager.GetInstance().GetPlayer();
        player.GetComponent<SpriteRenderer>().color = Color.clear;
        player.GetComponent<PlayerMovement>().canMove = false;
    }

    private void Update()
    {
        if(!begin)
            player.transform.position = transform.position;
    }

    public void LevelStart()
    {
        begin = true;
        player.GetComponent<SpriteRenderer>().color = Color.green;
        player.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.3f);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
        StartCoroutine(PlayerControl());

    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    IEnumerator PlayerControl()
    {
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<PlayerMovement>().canMove = true;
    }
}
