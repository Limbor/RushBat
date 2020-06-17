using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodyFangs : MonoBehaviour
{
    [SerializeField] private int hurtNumber;
    private int baseDamage;
    private float invalidTime;
    private Coroutine invalidProcess;
    private EnemyMovement enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        hurtNumber = 1;
        invalidTime = 2f;
        baseDamage = 10;
        enemy = gameObject.GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if(enemy.IsDead()) Destroy(this);
    }

    public float GetExtraDamage()
    {
        return hurtNumber / 5 * baseDamage;
    }
    
    public void AddHurtNumber()
    {
        hurtNumber += 1;
        if(invalidProcess != null) StopCoroutine(invalidProcess);
        invalidProcess = StartCoroutine(Invalid());
    }

    IEnumerator Invalid()
    {
        yield return new WaitForSeconds(invalidTime);
        hurtNumber = 0;
    }
}
