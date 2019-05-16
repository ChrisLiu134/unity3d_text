using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class SkillManager  {
    //单例
    private static SkillManager _instance;
    public static SkillManager Instance()
    {
        if (_instance==null)
        {
            _instance = new SkillManager();
        }
        return _instance;
    }


    #region  读取json文件中写好的技能表， 然后载入技能字典中，方便查找。
    /// <summary>
    /// 整个游戏所以技能的字典，方便查找和给角色添加技能。
    /// </summary>
    public Dictionary<int, Skill> skillDic = new Dictionary<int, Skill>();

    /// <summary>
    /// 读取json文件用
    /// </summary>
    JsonData jsonData;

    /// <summary>
    /// 读取技能json文件
    /// </summary>
    public void LoadSkillJson()
    {
        jsonData = JsonMapper.ToObject(Resources.Load<TextAsset>("Json/skill").text);
        var info = jsonData["info"];
        for (int i = 0; i < info.Count; i++)
        {
            int skilltype = (int)info[i]["skillType"];
            switch (skilltype)
            {
                case 0:
                    string name = info[i]["name"].ToString();
                    int id = (int)info[i]["id"];
                    string skillInfo = info[i]["skillInfo"].ToString();
                    int cd = (int)info[i]["cd"];
                    string effectPath = info[i]["effectPath"].ToString();
                    string spr = info[i]["spr"].ToString();
                    string skillClipPath = info[i]["skillClipPath"].ToString();
                    int damage = (int)info[i]["damage"];
                    int skillConsume = (int)info[i]["skillConsume"];
                    string animatorName = info[i]["animatorName"].ToString();
                    int skillDistance = (int)info[i]["skillDistance"];
                    Skill skill = Skill.MonomerDamageSkill(name, id, skillInfo, cd, effectPath, spr, skillClipPath, damage, skillConsume, animatorName, skillDistance);
                    skillDic.Add(skill.id, skill);
                    break;
                case 1:
                    string name1 = info[i]["name"].ToString();
                    int id1 = (int)info[i]["id"];
                    string skillInfo1 = info[i]["skillInfo"].ToString();
                    int cd1 = (int)info[i]["cd"];
                    string effectPath1 = info[i]["effectPath"].ToString();
                    string spr1 = info[i]["spr"].ToString();
                    string skillClipPath1 = info[i]["skillClipPath"].ToString();
                    int healHP1 = (int)info[i]["healHP"];
                    int healMP1 = (int)info[i]["healMP"];
                    int skillConsume1 = (int)info[i]["skillConsume"];
                    string animatorName1 = info[i]["animatorName"].ToString();
                    int skillDistance1 = (int)info[i]["skillDistance"];
                    Skill skill1 = Skill.MonomerTreatment(name1, id1, skillInfo1, cd1, effectPath1, spr1, skillClipPath1, healHP1, healMP1, skillConsume1, animatorName1,skillDistance1);
                    skillDic.Add(skill1.id, skill1);
                    break;
                case 2:
                    string name2 = info[i]["name"].ToString();
                    int id2 = (int)info[i]["id"];
                    string skillInfo2 = info[i]["skillInfo"].ToString();
                    int cd2 = (int)info[i]["cd"];
                    string effectPath2 = info[i]["effectPath"].ToString();
                    string spr2 = info[i]["spr"].ToString();
                    string skillClipPath2 = info[i]["skillClipPath"].ToString();
                    int healHP2 = (int)info[i]["healHP"];
                    int healMP2 = (int)info[i]["healMP"];
                    int attackBuff2 = (int)info[i]["attackBuff"];
                    int defBuff2 = (int)info[i]["defBuff"];
                    int attackSpeedBuff2 = (int)info[i]["attackSpeedBuff"];
                    int criticalAttribute2 = (int)info[i]["criticalAttribute"];
                    int evadeAttribute2 = (int)info[i]["evadeAttribute"];
                    int time2 = (int)info[i]["time"];
                    int skillConsume2 = (int)info[i]["skillConsume"];
                    string animatorName2 = info[i]["animatorName"].ToString();
                    int skillDistance2 = (int)info[i]["skillDistance"];
                    Skill skill2 = Skill.MonomerBuffSkill(name2, id2, skillInfo2, cd2, effectPath2, spr2, skillClipPath2, healHP2, healMP2, attackBuff2, defBuff2, attackSpeedBuff2, criticalAttribute2, evadeAttribute2, time2, skillConsume2, animatorName2,skillDistance2);
                    skillDic.Add(skill2.id, skill2);
                    break;
                case 3:
                    string name3 = info[i]["name"].ToString();
                    int id3 = (int)info[i]["id"];
                    string skillInfo3 = info[i]["skillInfo"].ToString();
                    int cd3 = (int)info[i]["cd"];
                    string effectPath3 = info[i]["effectPath"].ToString();
                    string spr3 = info[i]["spr"].ToString();
                    string skillClipPath3 = info[i]["skillClipPath"].ToString();
                    int damage3 = (int)info[i]["damage"];
                    int time3 = (int)info[i]["time"];
                    int range3 = (int)info[i]["range"];
                    int skillConsume3 = (int)info[i]["skillConsume"];
                    string animatorName3 = info[i]["animatorName"].ToString();
                    int skillDistance3 = (int)info[i]["skillDistance"];
                    Skill skill3 = Skill.GroupDamageSkill(name3, id3, skillInfo3, cd3, effectPath3, spr3, skillClipPath3, damage3, time3, range3, skillConsume3, animatorName3, skillDistance3);
                    skillDic.Add(skill3.id, skill3);
                    break;
                case 4:
                    //暂时没有这一类型的技能
                    break;
                case 5:
                    string name5 = info[i]["name"].ToString();
                    int id5 = (int)info[i]["id"];
                    string skillInfo5 = info[i]["skillInfo"].ToString();
                    int cd5 = (int)info[i]["cd"];
                    string effectPath5 = info[i]["effectPath"].ToString();
                    string spr5 = info[i]["spr"].ToString();
                    string skillClipPath5 = info[i]["skillClipPath"].ToString();
                    int healHP5 = (int)info[i]["healHP"];
                    int healMP5 = (int)info[i]["healMP"];
                    int attackBuff5 = (int)info[i]["attackBuff"];
                    int defBuff5 = (int)info[i]["defBuff"];
                    int attackSpeedBuff5 = (int)info[i]["attackSpeedBuff"];
                    int criticalAttribute5 = (int)info[i]["criticalAttribute"];
                    int evadeAttribute5 = (int)info[i]["evadeAttribute"];
                    int time5 = (int)info[i]["time"];
                    int range5 = (int)info[i]["range"];
                    int skillConsume5 = (int)info[i]["skillConsume"];
                    string animatorName5 = info[i]["animatorName"].ToString();
                    int skillDistance5 = (int)info[i]["skillDistance"];
                    Skill skill5 = Skill.GroupBuffSkill(name5, id5, skillInfo5, cd5, effectPath5, spr5, skillClipPath5, healHP5, healMP5, attackBuff5, defBuff5, attackSpeedBuff5, criticalAttribute5, evadeAttribute5, time5, range5, skillConsume5, animatorName5, skillDistance5);
                    skillDic.Add(skill5.id, skill5);
                    break;
                case 6:
                    string name6 = info[i]["name"].ToString();
                    int id6 = (int)info[i]["id"];
                    string skillInfo6 = info[i]["skillInfo"].ToString();
                    string spr6 = info[i]["spr"].ToString();
                    int criticalAttribute6 = (int)info[i]["criticalAttribute"];
                    int evadeAttribute6 = (int)info[i]["evadeAttribute"];
                    int powerAttribute6 = (int)info[i]["powerAttribute"];
                    int agilityAttribute6 = (int)info[i]["agilityAttribute"];
                    int mentalityAttribute6 = (int)info[i]["mentalityAttribute"];
                    Skill skill6 = Skill.AddAttributeSkill(name6, id6, skillInfo6, spr6, criticalAttribute6, evadeAttribute6,powerAttribute6, agilityAttribute6, mentalityAttribute6);
                    skillDic.Add(skill6.id, skill6);
                    break;
            }
        }
        Debug.Log("当前读取的技能数量" + skillDic.Count);
    }

    #endregion


    #region 关于技能 使用，和技能的buff状态
    /// <summary>
    ///剑客的技能列表
    /// </summary>
    public List<Skill> swordmanSkill = new List<Skill>();

    /// <summary>
    /// 法师的技能表
    /// </summary>
    public List<Skill> magicianSkill = new List<Skill>();

    /// <summary>
    /// 给玩家角色添加技能
    /// </summary>
    public void  PlayerChaGetSkill()
    {
        swordmanSkill.Add(skillDic[4]);
        swordmanSkill.Add(skillDic[5]);
        swordmanSkill.Add(skillDic[7]);
        swordmanSkill.Add(skillDic[8]);
        swordmanSkill.Add(skillDic[9]);
        swordmanSkill.Add(new Skill().Coby(skillDic[11]) );  //复制一个这个加属性的技能，因为这个技能 每个英雄都有

        magicianSkill.Add(skillDic[1]);
        magicianSkill.Add(skillDic[2]);
        magicianSkill.Add(skillDic[3]);
        magicianSkill.Add(skillDic[6]);
        magicianSkill.Add(skillDic[10]);
        magicianSkill.Add(new Skill().Coby(skillDic[11])); //复制一个这个加属性的技能，因为这个技能 每个英雄都有
    }

    /// <summary>
    /// 计算技能cd
    /// </summary>
    public void CountCd()
    {
        foreach (var skill in skillDic)
        {
            if (skill.Value.isCd == true)
            {
                skill.Value.cd_time += Time.deltaTime;
                if (skill.Value.cd_time>= skill.Value.cd)
                {
                    skill.Value.isCd = false;
                    skill.Value.cd_time = 0;
                }
            }
        }
    }

    /// <summary>
    /// 添加技能状态
    /// </summary>
    /// <returns></returns>
    public SkillState AddSkillState(Skill skill)
    {
        SkillState skillState = new SkillState();
        skillState.name = skill.name;
        skillState.info = skill.info;
        skillState.stateSpr = skill.skillSpr;
        skillState.buffskill = skill;
        skillState.time = skill.time;
        return skillState;


    }


    /// <summary>
    /// 结算状态,状态生效中。 只触发一次
    /// </summary>
    public void StatusAccounting(SkillState skillState, Charater charater)
    {
        charater.basicsAttack += skillState.buffskill.attackBuff;
        charater.basicsDef += skillState.buffskill.defBuff;
        charater.basicsAttackSpeed -= skillState.buffskill.attackSpeedBuff;
        charater.basicsCritical += skillState.buffskill.criticalAttribute;
        charater.basicsEvade += skillState.buffskill.evadeAttribute;
        charater.basicsHp += skillState.buffskill.healHP;
        charater.basicsMp += skillState.buffskill.healMP;
    }

    /// <summary>
    /// 移除状态
    /// </summary>
    public void RemoveState(SkillState skillState, Charater charater)
    {
        charater.basicsAttack -= skillState.buffskill.attackBuff;
        charater.basicsDef -= skillState.buffskill.defBuff;
        charater.basicsAttackSpeed += skillState.buffskill.attackSpeedBuff;
        charater.basicsCritical -= skillState.buffskill.criticalAttribute;
        charater.basicsEvade -= skillState.buffskill.evadeAttribute;
        charater.basicsHp -= skillState.buffskill.healHP;
        charater.basicsMp -= skillState.buffskill.healMP;
    }



    #endregion
}
