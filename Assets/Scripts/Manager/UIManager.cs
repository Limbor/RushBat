using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private static UIManager _manager;
    
    public Image dashIcon;
    public Image[] skill;
    public Image[] health;
    public Sprite[] hearts;
    public Image hurt;
    public Text coinNumber;
    public Text shieldNumber;
    public RawImage cover;

    private float fadeTime = 1.5f;
    

    private void Awake()
    {
        if(_manager == null)
        {
            _manager = this;
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        StartScene();
    }

    public void Hurt()
    {
        hurt.DOComplete();
        Tweener tween = hurt.DOFade(1f, 0.3f);
        tween.OnComplete(() =>
        {
            hurt.DOFade(0f, 0.3f);
        });
    }

    void StartScene()
    {
        cover.color = Color.black;
        cover.DOFade(0, fadeTime).OnComplete(() =>
        {
            cover.enabled = false;
        });
    }

    public void EndScene()
    {
        cover.enabled = true;
        cover.DOFade(1, fadeTime).OnComplete(() =>
        {
            GameManager.GetInstance().NextLevel();
        });
    }

    public void SetCoinNumber(int number)
    {
        coinNumber.text = "X" + number;
    }

    public void SetShieldNumber(int number)
    {
        shieldNumber.text = "X" + number;
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

    public void ResetDashTime(float percent = 1f)
    {
        dashIcon.fillAmount = percent;
    }

    public static UIManager GetInstance()
    {
        return _manager;
    }

    public void SetSkillTime(int index, float time)
    {
        skill[index].fillAmount -= time;
    }

    public void ResetSkillTime(int index, float percent = 1f)
    {
        skill[index].fillAmount = percent;
    }
}
