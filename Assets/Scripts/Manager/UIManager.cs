using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Pause Menu")] 
    public GameObject menu;

    private float fadeTime = 1.5f;
    [Space]
    public GameObject equipmentPanel;
    private Image shieldPrefab;
    private Image heartPrefab;
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
    }

    private void InitUI()
    {
        equipmentPanel = Resources.Load<GameObject>("Prefabs/UI/Profile");
        heartList = new List<Image>();
        shieldList = new List<Image>();
        heartPrefab = Resources.Load<Image>("Prefabs/UI/Heart");
        shieldPrefab = Resources.Load<Image>("Prefabs/UI/Shield");
        int heartNumber = Player.GetInstance().maxHealth / 4;
        for (int i = 0; i < heartNumber; i++)
        {
            AddHeart(i);
        }
    }

    private void AddHeart(int index)
    {
        Image heartUI = Instantiate(heartPrefab, transform.GetChild(0));
        var heart = heartUI.rectTransform;
        heart.SetAsFirstSibling();
        Vector2 pos = heart.position;
        float scaleInverse = heart.transform.parent.localScale.x;
        heart.position = new Vector2(pos.x + 60 * index * scaleInverse, pos.y);
        heartList.Add(heartUI);
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

    public void StartScene()
    {
        cover.color = Color.black;
        cover.DOFade(0, fadeTime).OnComplete(() =>
        {
            cover.enabled = false;
        });
    }

    public void EndScene(TweenCallback callBack)
    {
        cover.enabled = true;
        cover.DOFade(1, fadeTime).OnComplete(callBack);
    }

    public void ChangeMiniMap(Room room, bool enter = true)
    {
        var roomMap = room.transform.GetChild(0).gameObject;
        if (enter && room.FirstEnterRoom())
        {
            roomMap.SetActive(true);
            var connectedRooms = room.GetConnectedRoom();
            foreach (var thisRoomMap in 
                connectedRooms.Select(connectedRoom => connectedRoom.transform.GetChild(0).gameObject))
            {
                thisRoomMap.SetActive(true);
                thisRoomMap.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
        roomMap.GetComponent<SpriteRenderer>().color = enter ? Color.cyan : Color.gray;
    }
    
    public void ShowEquipmentInfo(string name)
    {
        EquipmentInfo equipmentInfo = GameManager.GetInstance().GetEquipmentInfo(name);
        var profile = Instantiate(equipmentPanel, transform.GetChild(0));
        profile.GetComponent<EquipmentDisplay>().SetContent(equipmentInfo);
    }

    public void SetCoinNumber(int number)
    {
        coinNumber.text = number.ToString();
    }
    
    public void SetKeyNumber(int number)
    {
        keyNumber.text = number.ToString();
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

    public void AddHeartContainer(int count)
    {
        for (var i = 0; i < count; i++)
        {
            int shieldCount = shieldList.Count;
            SetShieldNumber(0);
            AddHeart(heartList.Count + i);
            SetShieldNumber(shieldCount);
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
