using System;
using DG.Tweening;
using UnityEngine;


public class BigFireBall : MonoBehaviour
{
    private float speed = 8f;
    private float direction;
    private bool fly;
    private PlayerMovement playerMovement;
    private PlayerProperty playerProperty;
    private GameObject hit;

    private void Start()
    {
        hit = Resources.Load<GameObject>("Prefabs/FX/FireHit");
        playerMovement = GameManager.GetInstance().GetPlayer().GetComponent<PlayerMovement>();
        playerProperty = GameManager.GetInstance().GetPlayer().GetComponent<PlayerProperty>();
        direction = transform.localScale.x;
        fly = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GetComponent<SpriteRenderer>().DOFade(1, 2f).OnComplete(() =>
        {
            fly = true;
        });
    }

    private void Update()
    {
        if(!fly) return;
        transform.Translate(direction * Time.deltaTime * speed * Vector3.right);
    }
    
    private void FixedUpdate()
    {
        if(!fly) return;
        if (Physics2D.OverlapCircle(transform.position, 0.2f, 1 << LayerMask.NameToLayer("Ground")) != null)
        {
            Destroy(gameObject);
            Instantiate(hit, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!fly) return;
        if (other.CompareTag("Player"))
        {
            playerProperty.GetBurnt(3);
            playerMovement.Hurt(1, Vector2.right * direction, GameManager.Enemy);
            Destroy(gameObject);
            Instantiate(hit, transform.position, Quaternion.identity).GetComponent<DamageHero>().enabled = false;
        }
    }
}