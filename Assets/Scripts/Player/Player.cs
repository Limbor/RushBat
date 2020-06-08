using System;
using System.Collections.Generic;

// 单例模式Player类保存Player的相关属性数据
[Serializable]
public class Player
{
    private static Player _player = new Player();

    public int maxHealth = 16;
    public int currentHealth;
    public int shield;
    public int attack;
    public int coin;
    public int key;
    public List<string> equipments;
    public List<string> skills;
    public int lastPoisonedTime;
    public int lastBurntTime;
    
    public float dashCoolDown;
    public float skill1CoolDown;
    public float skill2CoolDown;
    public float skill3CoolDown;
    // 针对特殊装备的属性
    public bool hasRelived;
    public List<string> surroundingItems;

    private Player()
    {
        skills = new List<string>();
        Reset();
    }

    public void Reset()
    {
        maxHealth = 16;
        currentHealth = maxHealth;
        shield = 0;
        attack = 0;
        if (HaveSkill("Insurance")) coin = (int)(coin * 0.1);
        else coin = 0;
        key = 0;
        lastPoisonedTime = 0;
        lastBurntTime = 0;
        equipments = new List<string>();
        dashCoolDown = 0;
        skill1CoolDown = 0;
        skill2CoolDown = 0;
        skill3CoolDown = 0;

        hasRelived = false;
        surroundingItems = new List<string>();
    }
    
    public static Player GetInstance()
    {
        return _player;
    }
    
    public bool HaveSkill(string skillName)
    {
        foreach (var skill in skills)
        {
            if (skill.Equals(skillName)) return true;
        }
        return false;
    }

    public void SetPlayer(Player player)
    {
        maxHealth = player.maxHealth;
        currentHealth = player.currentHealth;
        shield = player.shield;
        attack = player.attack;
        coin = player.coin;
        key = player.key;
        lastPoisonedTime = player.lastPoisonedTime;
        lastBurntTime = player.lastBurntTime;
        player.equipments.ForEach(i => equipments.Add(i));
        dashCoolDown = player.dashCoolDown;
        skill1CoolDown = player.skill1CoolDown;
        skill2CoolDown = player.skill2CoolDown;
        skill3CoolDown = player.skill3CoolDown;
        hasRelived = player.hasRelived;
        player.surroundingItems.ForEach(i => surroundingItems.Add(i));
        player.skills.ForEach(i => skills.Add(i));
    }
}