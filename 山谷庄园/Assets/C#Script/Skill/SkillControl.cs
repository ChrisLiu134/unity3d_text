using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;

public class SkillControl : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler{
    //当前脚本储存的skill信息
    public Skill skill= new Skill();

    //技能信息展示窗口
    public GameObject infoView;
    public Text text;
    //
    //技能遮罩用的image
    public GameObject skillMask;

    /// <summary>
    /// 自己身上的button按钮
    /// </summary>
    Button btr;

    /// <summary>
    /// cd效果组件
    /// </summary>
    public Slider cdSlider;

    PlayerControl playerControl;

    // Use this for initialization
    void Start () {
        btr = GetComponent<Button>();
        btr.onClick.AddListener(CilckPlusPoint);
	}
	
	// Update is called once per frame
	void Update () {
        SkillsMask();
        ShowCountCD();
    }

    #region 显示技能信息方法
    /// <summary>
    /// 显示技能信息
    /// </summary>
    /// <param name="s"></param>
    /// <param name="sb"></param>
    public void ShowSkillInfo(Skill s , StringBuilder sb)
    {

        sb.Append("名字：" + s.name + "\n");
        sb.Append("简介：" + s.info + "\n");
        if (s.skillType==Skill.SkillType.AddAttribute)
        {
            sb.Append("每升一级增加3点力量，3点敏捷，3点智力，和1点暴击率，1点闪避率");
        }
        else
        {
            if (s.damage > 0)
            {
                sb.Append("伤害： " + s.damage + "\n");
            }

            if (s.healHP > 0)
            {
                sb.Append("恢复生命值：" + skill.healHP + "\n");
            }
            else if (skill.healHP < 0)
            {
                sb.Append("伤害生命值：" + (-skill.healHP) + "\n");
            }

            if (skill.healMP > 0)
            {
                sb.Append("恢复魔法值：" + skill.healMP + "\n");
            }
            else if (skill.healMP < 0)
            {
                sb.Append("伤害魔法值：" + (-skill.healMP) + "\n");
            }

            if (s.attackBuff > 0)
            {
                sb.Append("增加攻击力: " + s.attackBuff + "\n");
            }
            else if (s.attackBuff < 0)
            {
                sb.Append("减少攻击力: " + (-s.attackBuff) + "\n");
            }

            if (s.defBuff > 0)
            {
                sb.Append("增加防御力: " + s.defBuff + "\n");
            }
            else if (s.defBuff < 0)
            {
                sb.Append("减少防御力: " + (-s.defBuff) + "\n");
            }

            if (s.attackSpeedBuff < 0)
            {
                sb.Append("增加攻击速度: " + (-s.attackSpeedBuff) + "\n");
            }
            else if (s.attackSpeedBuff > 0)
            {
                sb.Append("减少攻击速度: " + s.attackSpeedBuff + "\n");
            }

            if (s.criticalAttribute > 0)
            {
                sb.Append("增加暴击几率: " + s.criticalAttribute + "\n");
            }
            else if (s.criticalAttribute < 0)
            {
                sb.Append("减少暴击几率: " + (-s.criticalAttribute) + "\n");
            }

            if (s.evadeAttribute > 0)
            {
                sb.Append("增加的闪避几率: " + s.evadeAttribute + "\n");
            }
            else if (s.evadeAttribute < 0)
            {
                sb.Append("减少闪避几率: " + (-s.evadeAttribute) + "\n");
            }

            if (s.time > 0)
            {
                sb.Append("法术持续时间：" + s.time + "\n");
            }

            if (s.range > 0)
            {
                sb.Append("作用范围：" + s.range + "码" + "\n");
            }

            sb.Append("cd: " + skill.cd + "\n");
            sb.Append("消耗: " + skill.skillConsume + "\n");
        }
    }

    /// <summary>
    /// 鼠标指向时
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        infoView.SetActive(true);
        StringBuilder sb = new StringBuilder();
        ShowSkillInfo(skill, sb);
        text.text = sb.ToString();
    }

    /// <summary>
    /// 鼠标离开时
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        infoView.SetActive(false);
        text.text = "";
    }
    #endregion

    #region 技能升级， 加点，  使用技能
    /// <summary>
    /// 技能是0级的时候让技能变黑,如果不是，就变白
    /// </summary>
    public void SkillsMask()
    {
        if (skill.level==0)
        {
            skillMask.SetActive(true);
        }
        else
        {
            skillMask.SetActive(false);
        }
        
    }

    /// <summary>
    /// 点击技能，加点
    /// </summary>
    public void CilckPlusPoint()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        if (playerControl==null)
        {
            Debug.Log("没找到当前玩家操控的角色");
            return;
        }
        if (playerControl.playerCha.player.skillPoints==0)
        {
            Debug.Log("没有技能点了");
            //这里接入使用技能的方法。 
            UseSkill();
            return;
        }

        //技能等级上限
        if (skill.skillType==Skill.SkillType.AddAttribute)
        {
            if (skill.level == 10)
            {
                UseSkill();
                return;
            }
        }
        else
        {
            if (skill.level==5)
            {
                UseSkill();
                return;
            }
        }

        //技能学习限制
        if (skill.skillType == Skill.SkillType.GroupTherapy|| skill.skillType == Skill.SkillType.GroupDamage|| skill.skillType == Skill.SkillType.GroupBuff)
        {
            if (playerControl.playerCha.player.level<4)
            {
                GameObject obj = Instantiate(playerControl.infoText, playerControl.transform.position + new Vector3(0, 2, 1), Quaternion.identity);
                obj.AddComponent<TextDamage>();
                obj.GetComponent<TextDamage>().GetInfo("等级不够,无法学习", Color.yellow);
                return;
            }
        }

        //技能升级的处理
        skill.level += 1;
        playerControl.playerCha.player.skillPoints -= 1;


        if (skill.skillType != Skill.SkillType.AddAttribute)  //如果学习的是普通的释放性的技能
        {
            if (skill.level < 2)
            {
                return;
            }
            //技能伤害各项能力增加
            if (skill.damage > 0)
            {
                skill.damage += skill.damage/skill.level ;
            }

            if (skill.healHP > 0)
            {
                skill.healHP += skill.healHP / skill.level;
            }
            else if (skill.healHP < 0)
            {
                skill.healHP += skill.healHP / skill.level;
            }

            if (skill.healMP > 0)
            {
                skill.healMP += skill.healMP / skill.level;
            }
            else if (skill.healMP < 0)
            {
                skill.healMP += skill.healMP / skill.level;
            }

            if (skill.attackBuff > 0)
            {
                skill.attackBuff += skill.attackBuff / skill.level;
            }
            else if (skill.attackBuff < 0)
            {
                skill.attackBuff += skill.attackBuff / skill.level;
            }

            if (skill.defBuff > 0)
            {
                skill.defBuff += skill.defBuff / skill.level;
            }
            else if (skill.defBuff < 0)
            {
                skill.defBuff += skill.defBuff/skill.level;
            }

            if (skill.criticalAttribute > 0)
            {
                skill.criticalAttribute += skill.criticalAttribute / skill.level;
            }
            else if (skill.criticalAttribute < 0)
            {
                skill.criticalAttribute += skill.criticalAttribute / skill.level;
            }

            if (skill.evadeAttribute > 0)
            {
                skill.criticalAttribute += skill.criticalAttribute / skill.level;
            }
            else if (skill.evadeAttribute < 0)
            {
                skill.criticalAttribute += skill.criticalAttribute / skill.level;
            }

            if (skill.time>0)
            {
                skill.time += 1;
            }
        }
        else  //如果我学的是加属性的技能
        {
            AddAttributeSkill();
        }

    }

    /// <summary>
    /// 被动的加属性的技能
    /// </summary>
    public void AddAttributeSkill()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        if (playerControl.currenPlayerSkills== SkillManager.Instance().swordmanSkill) //剑客
        {
            //因为我设定技能列表中下标为5的技能是加属性的技能。 因此这里这么写
            playerControl.playerCha.player.power += SkillManager.Instance().swordmanSkill[5].powerAttribute;
            playerControl.playerCha.player.agility += SkillManager.Instance().swordmanSkill[5].agilityAttribute;
            playerControl.playerCha.player.mentality += SkillManager.Instance().swordmanSkill[5].mentalityAttribute;
            playerControl.playerCha.player.basicsCritical += SkillManager.Instance().swordmanSkill[5].criticalAttribute;
            playerControl.playerCha.player.basicsEvade += SkillManager.Instance().swordmanSkill[5].evadeAttribute;

        }
        else if (playerControl.currenPlayerSkills == SkillManager.Instance().magicianSkill)//法师
        {
            playerControl.playerCha.player.power += SkillManager.Instance().magicianSkill[5].powerAttribute;
            playerControl.playerCha.player.agility += SkillManager.Instance().magicianSkill[5].agilityAttribute;
            playerControl.playerCha.player.mentality += SkillManager.Instance().magicianSkill[5].mentalityAttribute;
            playerControl.playerCha.player.basicsCritical += SkillManager.Instance().magicianSkill[5].criticalAttribute;
            playerControl.playerCha.player.basicsEvade += SkillManager.Instance().magicianSkill[5].evadeAttribute;
        }
        //刷新一次属性
        playerControl.playerCha.RefreshAttribute(); 
    }

    /// <summary>
    /// 使用技能时
    /// </summary>
    public void UseSkill()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        if (skill.level==0)
        {
            GameObject obj = Instantiate(playerControl.infoText, playerControl.transform.position + new Vector3(0, 2, 1), Quaternion.identity);
            obj.AddComponent<TextDamage>();
            obj.GetComponent<TextDamage>().GetInfo("技能还未学习，无法使用", Color.yellow);
            return;
        }
        if (playerControl.playerCha.player.mp <skill.skillConsume)
        {
            GameObject obj = Instantiate(playerControl.infoText, playerControl.transform.position + new Vector3(0, 2, 1), Quaternion.identity);
            obj.AddComponent<TextDamage>();
            obj.GetComponent<TextDamage>().GetInfo("魔法不够", Color.yellow);
            return;
        }
        if (skill.isCd)
        {
            GameObject obj = Instantiate(playerControl.infoText, playerControl.transform.position + new Vector3(0, 2, 1), Quaternion.identity);
            obj.AddComponent<TextDamage>();
            obj.GetComponent<TextDamage>().GetInfo("技能冷却中", Color.yellow);
            return;
        }
        if (skill.skillType==Skill.SkillType.AddAttribute)
        {
            Debug.Log("加属性的被动技能不能用");
            return;
        }

        playerControl.isUseSkillFindTarget = true;
        playerControl.currenUseSkill = skill;
    }

    /// <summary>
    /// 显示cd走圈圈
    /// </summary>
    public void ShowCountCD()
    {
        if (skill.isCd)
        {
            cdSlider.GetComponentInChildren<Image>().fillAmount = 1;
            skill.cdSliderStart = true;
            cdSlider.GetComponentInChildren<Image>().fillAmount -= skill.cd_time/skill.cd;
        }
        else
        {
            cdSlider.GetComponentInChildren<Image>().fillAmount = 0;
        }
    }

    #endregion
}
