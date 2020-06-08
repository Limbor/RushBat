using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public Text text;

    private Vector3 originScale;

    private void Awake()
    {
        originScale = text.transform.localScale;
    }

    void OnEnable()
    {
        StartCoroutine(ReturnPool());
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }

    public void SetNumber(float number)
    {
        text.text = ((int) number).ToString();
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }

    public void SetScale(float scale = 1.33f)
    {
        text.transform.localScale *= scale;
    }

    IEnumerator ReturnPool()
    {
        yield return new WaitForSeconds(lifeTime);
        text.transform.localScale = originScale;
        PoolManager.GetInstance().ReturnDamageTextPool(gameObject);
    }
}
