using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private static UIManager _manager;
    [Header("Skill Icon")]
    public Image dashIcon;
    public Image[] skill;
    [Header("Player UI")]
    public Sprite[] hearts;
    public Image hurt;
    public Text coinNumber;
    public Text keyNumber;
    [Header("Scene Fade")]
    public RawImage cover;
    [Header("Equipment Panel")]
    public GameObject equipmentPanel;
    public Image equipmentImage;
    public Text equipmentName;
    public Text equipmentIntro;

    private float fadeTime = 1.5f;
    private Image shieldPrefab;
    private Image heartPrefab;
    private Dictionary<string, EquipmentInfo> equipmentInfoMap;
    private List<Image> heartList;
    private List<Image> shieldList;

    private void Awake()
    {
        if(_manager == null)
        {
            _manager = this;
            InitUI();
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        StartScene();
        ReadAssets();
    }

    private void InitUI()
    {
        heartList = new List<Image>();
        shieldList = new List<Image>();
        heartPrefab = Resources.Load<Image>("Prefabs/UI/Heart");
        shieldPrefab = Resources.Load<Image>("Prefabs/UI/Shield");
        int heartNumber = Player.GetInstance().maxHealth / 4;
        for (int i = 0; i < heartNumber; i++)
        {
            Image heartUI = Instantiate(heartPrefab, transform.GetChild(0));
            var heart = heartUI.rectTransform;
            heart.SetAsFirstSibling();
            Vector2 pos = heart.position;
            float scaleInverse = heart.transform.parent.localScale.x;
            heart.position = new Vector2(pos.x + 60 * i * scaleInverse, pos.y);
            heartList.Add(heartUI);
        }
    }
    
    private void ReadAssets()
    {
        TextAsset text = Resources.Load<TextAsset>("TextAssets/Equipment");
        EquipmentInfoList equipmentInfoList = JsonUtility.FromJson<EquipmentInfoList>(text.text);
        equipmentInfoMap = new Dictionary<string, EquipmentInfo>();
        foreach (var equipmentInfo in equipmentInfoList.equipmentList)
        {
            equipmentInfoMap.Add(equipmentInfo.enName, equipmentInfo);
        }
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

    public void ShowEquipmentInfo(string name)
    {
        EquipmentInfo equipmentInfo = equipmentInfoMap[name];
        equipmentName.text = equipmentInfo.name;
        equipmentIntro.text = equipmentInfo.intro;
        equipmentImage.sprite = Resources.Load<Sprite>("Images/" + equipmentInfo.enName);
        equipmentPanel.SetActive(true);
        StartCoroutine(HideEquipmentInfo());
    }

    IEnumerator HideEquipmentInfo()
    {
        yield return new WaitForSeconds(1.5f);
        equipmentPanel.SetActive(false);
    }
    
    public void SetCoinNumber(int number)
    {
        coinNumber.text = number.ToString();
    }

    public void SetShieldNumber(int shieldNumber)
    {
        if (shieldNumber > shieldList.Count)
        {
            for (int i = shieldList.Count; i < shieldNumber; i++)
            {
                Image shieldtUI = Instantiate(shieldPrefab, transform.GetChild(0));
                var shield = shieldtUI.rectTransform;
                shield.SetAsFirstSibling();
                Vector2 pos = shield.position;
                float scaleInverse = shield.transform.parent.localScale.x;
                shield.position = new Vector2(pos.x + 60 * (i + heartList.Count) * scaleInverse, pos.y);
                shieldList.Add(shieldtUI);
            }
        }
        else
        {
            for (int i = shieldList.Count - 1; i >= shieldNumber; i--)
            {
                Destroy(shieldList[i].gameObject);
                shieldList.RemoveAt(i);
            }
        }
    }

    public void SetPlayerHealth(int health)
    {
        foreach(Image heart in heartList)
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
