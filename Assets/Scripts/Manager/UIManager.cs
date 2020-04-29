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
    public RawImage fader;

    private void Awake()
    {
        if(_manager == null)
        {
            _manager = this;
            return;
        }
        Destroy(gameObject);
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

    public void EndScene()
    {
        fader.GetComponent<SceneFader>().EndScene();
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
        return _manager;
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
