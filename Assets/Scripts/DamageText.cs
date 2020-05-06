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
    
    void OnEnable()
    {
        StartCoroutine(ReturnPool());
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    public void SetNumber(float number)
    {
        text.text = ((int) number).ToString();
    }

    IEnumerator ReturnPool()
    {
        yield return new WaitForSeconds(1f);
        PoolManager.GetInstance().ReturnDamageTextPool(gameObject);
    }
}
