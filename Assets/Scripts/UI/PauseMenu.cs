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
    [Space]
    public GameObject equipmentPanel;
    public Button[] mainButton;
    [Space]
    public Slider audioSlider;

    private List<GameObject> equipmentList;
    private GameObject equipmentGrid;
    private bool isShowing = false;
    
    private void Awake()
    {
        equipmentGrid = Resources.Load<GameObject>("Prefabs/UI/EquipmentBox");
        switchButton[0].onClick.AddListener(() =>
        { 
            subMenu[0].SetActive(true);
            subMenu[1].SetActive(false);
        });
        switchButton[1].onClick.AddListener(() =>
        { 
            subMenu[1].SetActive(true);
            ShowEquipmentList();
            subMenu[0].SetActive(false);
        });
        
        mainButton[0].onClick.AddListener(Resume);
        mainButton[1].onClick.AddListener(AudioSet);
        mainButton[2].onClick.AddListener(Quit);
    }
    private void OnEnable()
    {
        equipmentList = new List<GameObject>();
        switchButton[0].Select();
        subMenu[0].SetActive(true);
    }


    private void OnDisable()
    {
        subMenu[0].SetActive(false);
        subMenu[1].SetActive(false);
        foreach (var equipment in equipmentList)
        {
            Destroy(equipment);
        }
        isShowing = false;
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        subMenu[0].SetActive(false);
        subMenu[1].SetActive(false);
        foreach (var equipment in equipmentList)
        {
            Destroy(equipment);
        }
        UIManager.GetInstance().menu.SetActive(false);
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void AudioSet()
    {
        audioSlider.gameObject.SetActive(!audioSlider.gameObject.activeSelf);
    }

    private void ShowEquipmentList()
    {
        if(isShowing) return;
        isShowing = true;
        var playerEquipment = Player.GetInstance().equipments;
        foreach (var equipment in playerEquipment)
        {
            var grid = Instantiate(equipmentGrid, equipmentPanel.transform);
            grid.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>("Images/" + equipment);
            equipmentList.Add(grid);
            grid.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                ShowEquipmentInfo(equipment);
            });
        }
        if(playerEquipment.Count > 0) ShowEquipmentInfo(playerEquipment[0]);
    }

    private void ShowEquipmentInfo(string equipmentName)
    {
        var texts = subMenu[1].GetComponentsInChildren<Text>();
        var info = GameManager.GetInstance().GetEquipmentInfo(equipmentName);
        texts[0].text = info.name;
        texts[1].text = info.intro;
    }
}
