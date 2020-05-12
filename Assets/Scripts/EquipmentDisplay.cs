using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EquipmentDisplay : MonoBehaviour
{
    private Image image;
    private Text name;
    private Text intro;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().DOFade(0.8f, 0.5f);
        Destroy(gameObject, 2f);
    }

    public void SetContent(EquipmentInfo equipmentInfo)
    {
        image = GetComponentsInChildren<Image>()[1];
        Text[] childList = GetComponentsInChildren<Text>();
        name = childList[0];
        intro = childList[1];
        name.text = equipmentInfo.name;
        intro.text = equipmentInfo.intro;
        image.sprite = Resources.Load<Sprite>("Images/" + equipmentInfo.enName);
    }

}
