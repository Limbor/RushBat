using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    public Text skillName;
    public Text price;
    public Text state;

    public Button view;
    public Button buy;
    
    private SkillBoard board;
    private Skill skill;
    private PlayerProperty player;

    private bool bought;
    // Start is called before the first frame update
    void Start()
    {
        skillName.text = skill.name;
        price.text = skill.levelList[0].price.ToString();
        state.text = "已习得";
        player = GameManager.GetInstance().GetPlayer().GetComponent<PlayerProperty>();
        view.onClick.AddListener(() =>
        {
            board.SetIntro(skill.levelList[0].intro);
        });
        
        if(!bought)
        {
            state.text = "购买";
            buy.onClick.AddListener(() =>
            {
                if (player.GetCoinNumber() < skill.levelList[0].price) return;
                player.SetCoinNumber(-skill.levelList[0].price);
                player.AddSkill(skill.enName);
                state.text = "已习得";
            });
        }
    }

    public void SetItem(Skill skillInfo, SkillBoard board)
    {
        this.board = board;
        skill = skillInfo;
    }

    public void HaveBought()
    {
        bought = true;
    }
}
