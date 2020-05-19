using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerProperty : MonoBehaviour
{
    [Header("State Effect")]
    public GameObject poisonPartical;
    public GameObject burnPartical;

    [Header("Skill CD")]
    public float dashCoolDown = 2f;
    public float skill2CoolDown = 5f;
    public float skill1CoolDown = 3f;
    public float skill3CoolDown = 10f;

    [Header("Player State")]
    public bool isDead = false;
    public bool isPoisoned = false;
    public bool isBurnt = false;

    private int maxHealth;
    private int currentHealth;
    private int attack;
    private int shield;
    private int coin;
    private int key;
    [SerializeField]
    private List<string> equipments;
    private int lastPoisonedTime;
    private int lastBurntTime;

    private PlayerAnimation anim;
    private Rigidbody2D rb;
    private Player player;
    private GameObject healFx;
    private GameObject reliveFx;
    void Start()
    {
        player = Player.GetInstance();
        // 复原player基础属性
        maxHealth = player.maxHealth;
        currentHealth = player.currentHealth;
        shield = player.shield;
        attack = player.attack;
        coin = player.coin;
        key = player.key;
        
        lastPoisonedTime = player.lastPoisonedTime;
        if(lastPoisonedTime != 0) GetPoisoned(0);
        lastBurntTime = player.lastBurntTime;
        if(lastBurntTime != 0) GetBurnt(0);
        
        equipments = player.equipments;
        // 恢复环绕型的道具
        foreach (var item in player.surroundingItems)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Item/" + item));
        }

        anim = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        healFx = Resources.Load<GameObject>("Prefabs/FX/Heal");
        reliveFx = Resources.Load<GameObject>("Prefabs/FX/Relive");

        UIManager.GetInstance().SetPlayerHealth(currentHealth);
        UIManager.GetInstance().SetCoinNumber(coin);
        UIManager.GetInstance().SetShieldNumber(shield);
        UIManager.GetInstance().SetKeyNumber(key);
    }

    private void Update()
    {
        if (isDead) return;
        player.currentHealth = currentHealth;
        player.shield = shield;
        player.attack = attack;
        player.coin = coin;
        player.key = key;
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
        SpecialEquipmentCheck(equipment);
        equipments.Add(equipment);
        anim.Acquire();
    }

    private void SpecialEquipmentCheck(string equipment)
    {
        if (equipment.Equals("HappyBeer") || equipment.Equals("IceCream"))
        {
            maxHealth += 4;
            player.maxHealth += 4;
            UIManager.GetInstance().AddHeartContainer(1);
            SetHealth(4);
        }
        else if(equipment.Equals("CrossBlade"))
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Item/" + equipment));
            player.surroundingItems.Add(equipment);
        }
        else if (equipment.Equals("ShadowBlade") || equipment.Equals("WizardSword"))
        {
            attack += 5;
        }
        else if (equipment.Equals("LotteryTicket"))
        {
            SetCoinNumber(999);
        }
    }

    public bool HaveEquipment(string equipmentName)
    {
        foreach (var equipment in equipments)
        {
            if (equipment.Equals(equipmentName)) return true;
        }
        return false;
    }

    public void RemoveEquipment(string equipmentName)
    {
        for (int i = 0; i < equipments.Count; i++)
        {
            if(equipments[i].Equals(equipmentName)) equipments.RemoveAt(i);
        }
    }

    public void SetCoinNumber(int change)
    {
        coin += change;
        coin = Mathf.Clamp(coin, 0, 999);
        if (change > 0 && HaveEquipment("SapphireRing"))
        {
            if(Random.Range(0, 1f) < 0.1f) SetShield(1);
        }
        UIManager.GetInstance().SetCoinNumber(coin);
    }
    
    public void SetKeyNumber(int change)
    {
        key += change;
        UIManager.GetInstance().SetKeyNumber(key);
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
            if (currentHealth != 0)
            {
                Instantiate(healFx, transform.position + Vector3.up * 0.2f, Quaternion.identity, transform);
            }
            else
            {
                Instantiate(reliveFx, transform.position + Vector3.up * 0.2f, Quaternion.identity, transform);
            }
        }
        currentHealth += change;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth == 0)
        {
            GetBurnt(-1);
            GetPoisoned(-1);
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
        if (HaveEquipment("GasMask")) return;
        if (GetComponent<PlayerMovement>().avoidDamage) return;
        if (time == -1)
        {
            lastPoisonedTime = 0;
            isPoisoned = false;
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
            if (HaveEquipment("GasMask")) lastPoisonedTime = 0;
            if(lastPoisonedTime == 0) break;
            lastPoisonedTime -= 1;
            Hurt(1);
            UIManager.GetInstance().Hurt();
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
        if (HaveEquipment("WeldingMask")) return;
        if (GetComponent<PlayerMovement>().avoidDamage) return;
        if (time == -1)
        {
            lastBurntTime = 0;
            isBurnt = false;
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
            if (HaveEquipment("WeldingMask")) lastBurntTime = 0;
            if(lastBurntTime == 0) break;
            lastBurntTime -= 1;
            Hurt(1);
            UIManager.GetInstance().Hurt();
        }
        isBurnt = false;
        burnPartical.SetActive(false);
    }

    public bool IsHealthy()
    {
        return currentHealth == maxHealth;
    }

    public int GetCoinNumber()
    {
        return coin;
    }
    
    public int GetKeyNumber()
    {
        return key;
    }

    public int GetAttack()
    {
        return attack;
    }
}
