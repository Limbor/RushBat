using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerProperty : MonoBehaviour
{
    [Header("State Icon")]
    public GameObject poisonPartical;
    public GameObject burnPartical;
    public GameObject healFx;

    [Header("Skill CD")]
    public float dashCoolDown = 2f;
    public float skill2CoolDown = 5f;
    public float skill1CoolDown = 3f;
    public float skill3CoolDown = 10f;

    [Header("Player State")]
    public SpriteRenderer state;
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
    }

    public void SetHealth(int change)
    {
        if (isDead) return;
        if(currentHealth != maxHealth && change > 0)
        {
            Instantiate(healFx, transform.position + Vector3.up * 0.2f, Quaternion.identity, transform);

        }
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
        poisonPartical.SetActive(true);
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
            UIManager.GetInstance().Hurt();
            SetHealth(-1);
            lastPoisonedTime -= 1;
        }
        isPoisoned = false;
        poisonPartical.SetActive(false);
    }

    public void GetBurnt(int time)
    {
        if (GetComponent<PlayerMovement>().avoidDamage) return;
        lastBurntTime += time;
        burnPartical.SetActive(true);
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
            UIManager.GetInstance().Hurt();
            SetHealth(-1);
            lastBurntTime -= 1;
        }
        isBurnt = false;
        burnPartical.SetActive(false);
    }
}
