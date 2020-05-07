using System.Collections.Generic;

// 单例模式Player类保存Player的相关属性数据
public class Player
{
    private static Player _player = new Player();

    public int maxHealth = 16;
    public int currentHealth;
    public int shield;
    public int coin;
    public List<string> equipments;
    public int lastPoisonedTime;
    public int lastBurntTime;
    
    public float dashCoolDown;
    public float skill1CoolDown;
    public float skill2CoolDown;
    public float skill3CoolDown;

    private Player()
    {
        Reset();
    }

    public void Reset()
    {
        currentHealth = maxHealth;
        shield = 0;
        coin = 0;
        lastPoisonedTime = 0;
        lastBurntTime = 0;
        equipments = new List<string>();
        dashCoolDown = 0;
        skill1CoolDown = 0;
        skill2CoolDown = 0;
        skill3CoolDown = 0;
    }
    
    public static Player GetInstance()
    {
        return _player;
    }
}