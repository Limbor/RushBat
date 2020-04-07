using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerProperty : MonoBehaviour
{
    [Header("State Icon")]
    public Sprite poison;

    [Header("Skill CD")]
    public float dashCoolDown = 2f;
    public float skill2CoolDown = 5f;
    public float skill1CoolDown = 3f;
    public float skill3CoolDown = 10f;

    [Header("Player State")]
    public GameObject state;
    public bool isDead = false;
    public int maxHealth = 16;
    public bool isPoisoned = false;

    private int currentHealth;
    private int lastPoisonedTime;

    void Start()
    {
        currentHealth = maxHealth;
        lastPoisonedTime = 0;
    }

    private void Update()
    {
        UIManager.GetInstance().SetPlayerHealth(currentHealth);
        UIManager.GetInstance().SetDashTime(1.0f / dashCoolDown * Time.deltaTime);
        UIManager.GetInstance().SetSkillTime(0, 1.0f / skill1CoolDown * Time.deltaTime);
        UIManager.GetInstance().SetSkillTime(1, 1.0f / skill2CoolDown * Time.deltaTime);
        UIManager.GetInstance().SetSkillTime(2, 1.0f / skill3CoolDown * Time.deltaTime);
        if (isPoisoned)
        {
            state.GetComponent<SpriteRenderer>().sprite = poison;
        }
        else
        {
            state.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public void SetHealth(int change)
    {
        if (isDead) return;
        currentHealth += change;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth == 0)
        {
            GetComponent<PlayerAnimation>().Die();
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            isDead = true;
        }
    }

    public void GetPoisoned(int time)
    {
        lastPoisonedTime += time;
        if (!isPoisoned)
        {
            StartCoroutine(Poisoned());
        }
    }

    IEnumerator Poisoned()
    {
        isPoisoned = true;
        while(lastPoisonedTime != 0)
        {
            yield return new WaitForSeconds(2f);
            GetComponent<SpriteRenderer>().color = new Color(0.153f, 0.255f, 0.176f);
            GetComponent<SpriteRenderer>().DOColor(Color.white, 0.5f);
            SetHealth(-1);
            lastPoisonedTime -= 1;
        }
        isPoisoned = false;
    }
}
