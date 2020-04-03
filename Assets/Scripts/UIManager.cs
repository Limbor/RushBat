using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager manager;
    public Image dashIcon;
    public Image[] skill;
    public Image[] health;
    public Sprite[] hearts;

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
        }
    }

    public void SetPlayerHealth(int health)
    {
        foreach(Image heart in this.health)
        {
            if(health >= 4)
            {
                heart.sprite = hearts[0];
                health -= 4;
            }
            else
            {
                heart.sprite = hearts[4 - health];
                health = 0;
            }
        }
    }

    public void SetDashTime(float time)
    {
        dashIcon.fillAmount -= time;
    }

    public void ResetDashTime()
    {
        dashIcon.fillAmount = 1f;
    }

    public static UIManager GetInstance()
    {
        return manager;
    }

    public void SetSkillTime(int index, float time)
    {
        skill[index].fillAmount -= time;
    }

    public void ResetSkillTime(int index)
    {
        skill[index].fillAmount = 1f;
    }
}
