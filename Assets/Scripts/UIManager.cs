using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager manager;
    public Image dashIcon;

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
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
}
