using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player player;

    [Header("Skill CD")]
    public float dashCoolDown = 2f;
    public float skill2CoolDown = 5f;
    public float skill1CoolDown = 3f;
    public float skill3CoolDown = 10f;

    [Header("Player State")]
    public bool isDead = false;
    public int maxHealth = 16;
    private int currentHealth;

    void Awake()
    {
        if (player == null)
        {
            player = this;
            currentHealth = maxHealth;
        }
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
        currentHealth += change;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (currentHealth < 0) currentHealth = 0;
        if(currentHealth == 0)
        {
            GetComponent<PlayerAnimation>().Die();
            isDead = true;
        }
    }

    public static Player GetInstance()
    {
        return player;
    }

}
