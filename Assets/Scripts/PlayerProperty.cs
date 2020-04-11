using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerProperty : MonoBehaviour
{
    [Header("State Icon")]
    public Sprite poison;
    public Sprite burn;

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
    public bool isBurnt = false;

    private int currentHealth;
    private int lastPoisonedTime;
    private int lastBurntTime;

    void Start()
    {
        currentHealth = maxHealth;
        lastPoisonedTime = 0;
        lastBurntTime = 0;
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
        else if (isBurnt)
        {
            state.GetComponent<SpriteRenderer>().sprite = burn;
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
        if (GetComponent<PlayerMovement>().avoidDamage) return;
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
            yield return new WaitForSeconds(1f);
            GetComponent<SpriteRenderer>().color = new Color(0.153f, 0.255f, 0.176f);
            GetComponent<SpriteRenderer>().DOColor(Color.white, 0.8f);
            SetHealth(-1);
            lastPoisonedTime -= 1;
        }
        isPoisoned = false;
    }

    public void GetBurnt(int time)
    {
        if (GetComponent<PlayerMovement>().avoidDamage) return;
        lastBurntTime += time;
        if (!isBurnt)
        {
            StartCoroutine(Burnt());
        }
    }

    IEnumerator Burnt()
    {
        isBurnt = true;
        while (lastBurntTime != 0)
        {
            yield return new WaitForSeconds(1f);
            GetComponent<SpriteRenderer>().color = new Color(0.416f, 0.125f, 0.180f);
            GetComponent<SpriteRenderer>().DOColor(Color.white, 0.8f);
            SetHealth(-1);
            lastBurntTime -= 1;
        }
        isBurnt = false;
    }
}
