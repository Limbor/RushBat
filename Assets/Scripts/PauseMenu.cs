using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button[] switchButton;
    public GameObject[] subMenu;

    public Button[] mainButton;
    public Slider audioSlider;
    
    private void OnEnable()
    {
        switchButton[0].Select();
        subMenu[0].SetActive(true);
        switchButton[0].onClick.AddListener(() =>
        { 
            subMenu[0].SetActive(true);
            subMenu[1].SetActive(false);
        });
        switchButton[1].onClick.AddListener(() =>
        { 
            subMenu[1].SetActive(true);
            subMenu[0].SetActive(false);
        });
        
        mainButton[0].onClick.AddListener(Resume);
        mainButton[1].onClick.AddListener(AudioSet);
        mainButton[2].onClick.AddListener(Quit);
    }

    void Resume()
    {
        Time.timeScale = 1f;
        subMenu[0].SetActive(false);
        subMenu[1].SetActive(false);
        UIManager.GetInstance().menu.SetActive(false);
    }

    void Quit()
    {
        Application.Quit();
    }

    void AudioSet()
    {
        audioSlider.gameObject.SetActive(!audioSlider.gameObject.activeSelf);
    }
}
