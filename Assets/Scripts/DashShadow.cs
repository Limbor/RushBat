using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashShadow : MonoBehaviour
{
    private Transform playerTransform;
    private SpriteRenderer render;

    public float activeTime = 1f;
    private float activeStart = 0f;

    public float alphaInit;
    private float alpha;
    public float alphaMultiplier;


    // Start is called before the first frame update
    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;

        render = GetComponent<SpriteRenderer>();
        render.sprite = player.GetComponent<SpriteRenderer>().sprite;

        transform.position = playerTransform.position;
        transform.localScale = playerTransform.localScale;
        transform.rotation = playerTransform.rotation;

        alpha = alphaInit;
        render.color = new Color(0f, 0f, 0f, alpha);
        activeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= activeStart + activeTime)
        {
            PoolManager.GetInstance().ReturnShadowPool(gameObject);
        }
        else
        {
            alpha *= alphaMultiplier;
            render.color = new Color(0f, 0f, 0f, alpha);
        }
    }
}
