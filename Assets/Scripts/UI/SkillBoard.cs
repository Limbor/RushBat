using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoard : MonoBehaviour
{
    private Text intro;
    private GameObject list;
    private List<GameObject> skillList;
    // Start is called before the first frame update
    private void Start()
    {
        var close = GetComponentInChildren<Button>();
        close.onClick.AddListener(() =>
        {
            InputManager.SetInputStatus(true);
            Destroy(gameObject);
        });
        
        skillList = new List<GameObject>();
        intro = GetComponentsInChildren<Text>()[1];
        list = GetComponentsInChildren<Image>()[1].gameObject;
        var skillPrefab = Resources.Load<GameObject>("Prefabs/UI/Skill");
        var player = GameManager.GetInstance().GetPlayer().GetComponent<PlayerProperty>();
        foreach (var skill in GameManager.GetInstance().GetSkillList())
        {
            var skillInfo = GameManager.GetInstance().GetSkillInfo(skill);
            var skillItem = Instantiate(skillPrefab, list.transform);
            skillItem.GetComponent<SkillItem>().SetItem(skillInfo, this);
            if (player.HaveSkill(skill))
            {
                skillItem.GetComponent<SkillItem>().HaveBought();
            }
            skillList.Add(skillItem);
        }
    }

    public void SetIntro(string text)
    {
        intro.text = text;
    }
}
