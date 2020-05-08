using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerProperty : MonoBehaviour
{
    [Header("State Effect")]
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
    public bool isPoisoned = false;
    public bool isBurnt = false;

    private int maxHealth;
    private int currentHealth;
    private int shield;
    private int coin;
    [SerializeField]
    private List<string> equipments;
    private int lastPoisonedTime;
    private int lastBurntTime;

    private PlayerAnimation anim;
    private Rigidbody2D rb;
    private Player player;
    
    void Start()
    {
        player = Player.GetInstance();
        
        maxHealth = player.maxHealth;
        currentHealth = player.currentHealth;
        shield = player.shield;
        coin = player.coin;
        
        lastPoisonedTime = player.lastPoisonedTime;
        if(lastPoisonedTime != 0) GetPoisoned(0);
        lastBurntTime = player.lastBurntTime;
        if(lastBurntTime != 0) GetBurnt(0);
        
        equipments = player.equipments;

        anim = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        
        UIManager.GetInstance().SetPlayerHealth(currentHealth);
        UIManager.GetInstance().SetCoinNumber(coin);
        UIManager.GetInstance().SetShieldNumber(shield);
    }

    private void Update()
    {
        if (isDead) return;
        player.currentHealth = currentHealth;
        player.shield = shield;
        player.coin = coin;
        player.lastPoisonedTime = lastPoisonedTime;
        player.lastBurntTime = lastBurntTime;
        player.equipments = equipments;
        
        UIManager.GetInstance().SetDashTime(1.0f / dashCoolDown * Time.deltaTime);
        UIManager.GetInstance().SetSkillTime(0, 1.0f / skill1CoolDown * Time.deltaTime);
        UIManager.GetInstance().SetSkillTime(1, 1.0f / skill2CoolDown * Time.deltaTime);
        UIManager.GetInstance().SetSkillTime(2, 1.0f / skill3CoolDown * Time.deltaTime);
    }

    public void Equip(string equipment)
    {
        if (HaveEquipment(equipment)) return;
        equipments.Add(equipment);
        anim.Acquire();
    }

    public bool HaveEquipment(string name)
    {
        foreach (var equipment in equipments)
        {
            if (equipment.Equals(name)) return true;
        }
        return false;
    }

    public void SetCoinNumber(int change)
    {
        coin += change;
        UIManager.GetInstance().SetCoinNumber(coin);
    }
    
    public void Hurt(int damage)
    {
        if (isDead) return;
        if(shield > damage) SetShield(-damage);
        else
        {
            SetHealth(shield - damage);
            SetShield(-shield);
        }
    }
    
    public void SetShield(int change)
    {
        if (isDead) return;
        shield += change;
        UIManager.GetInstance().SetShieldNumber(shield);
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
            anim.Die();
            rb.velocity = new Vector2(0, 0);
            isDead = true;
        }
        UIManager.GetInstance().SetPlayerHealth(currentHealth);
    }
    
    /// <summary>
    /// player中毒
    /// </summary>
    /// <param name="time">中毒时长</param>
    public void GetPoisoned(int time)
    {
        if (isDead) return;
        if (GetComponent<PlayerMovement>().avoidDamage) return;
        if (time == -1)
        {
            lastPoisonedTime = 0;
            return;
        }
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

    /// <summary>
    /// player烧伤
    /// </summary>
    /// <param name="time">烧伤时长</param>
    public void GetBurnt(int time)
    {
        if (isDead) return;
        if (GetComponent<PlayerMovement>().avoidDamage) return;
        if (time == -1)
        {
            lastBurntTime = 0;
            return;
        }
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
