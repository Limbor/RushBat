using System.Collections.Generic;

// 单例模式Player类保存Player的相关属性数据
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
    public int lastPoisonedTime;
    public int lastBurntTime;
    
    public float dashCoolDown;
    public float skill1CoolDown;
    public float skill2CoolDown;
    public float skill3CoolDown;
    // 针对特殊装备的属性
    public bool hasRelived;
    public List<string> surroundingItems = new List<string>();

    private Player()
    {
        Reset();
    }

    public void Reset()
    {
        maxHealth = 16;
        currentHealth = maxHealth;
        shield = 0;
        attack = 0;
        coin = 0;
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
}