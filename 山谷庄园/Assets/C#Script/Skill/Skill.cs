using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能信息类
/// </summary>
public class Skill
{
    /*
    1、技能信息：描述技能名、技能介绍、有效距离、播放动作、技能效果等信息
    2、效果信息：描述一个具名效果的执行操作列表、操作的作用目标等
    3、各个具体类型的效果信息：对导弹、连锁、状态、吟唱、地点之类的效果针对性进行描述其特征以及产生的效果（返回到第2点）

    从流程上看，从开始释放一个技能到效果完全结束大致经历这么几个步骤：
    1、发出施放请求
    2、验证是否满足使用技能条件
    3、返回失败结果或者开始执行技能同时开始动作、特效播放
    4、执行该技能需要表现的各项效果
    5、如需伤害判定则进行判断并反馈结果
    */

    //技能类型有  单体伤害，  单体治疗， 单体buff， 群体伤害， 群体治疗， 群体buff, 加属性.
    #region  技能的各种字段
    /// <summary>
    /// 技能类型
    /// </summary>
    public enum SkillType
    {
        /// <summary>
        /// 单体伤害技能
        /// </summary>
        MonomerDamage,
        /// <summary>
        /// 单体治疗技能
        /// </summary>
        MonomerTreatment,
        /// <summary>
        /// 单体buff技能
        /// </summary>
        MonomerBuff,
        /// <summary>
        /// 群体伤害技能
        /// </summary>
        GroupDamage,
        /// <summary>
        /// 群体治疗技能
        /// </summary>
        GroupTherapy,
        /// <summary>
        /// 群体buff技能
        /// </summary>
        GroupBuff,
        /// <summary>
        /// 加属性的技能
        /// </summary>
        AddAttribute
    }

    public SkillType skillType;

    /// <summary>
    /// 技能名字
    /// </summary>
    public string name;

    /// <summary>
    /// 技能编号
    /// </summary>
    public int id;

    /// <summary>
    /// 技能介绍
    /// </summary>
    public string info;

    /// <summary>
    /// 技能等级
    /// </summary>
    public int level;

    /// <summary>
    /// 持续时间
    /// </summary>
    public int time;

    /// <summary>
    /// 冷却时间
    /// </summary>
    public float cd;

    /// <summary>
    /// 技能作用范围
    /// </summary>
    public float range;

    /// <summary>
    /// 技能的特效组件
    /// </summary>
    public GameObject skillEffect;

    /// <summary>
    /// 技能的贴图路径
    /// </summary>
    public Sprite skillSpr;

    /// <summary>
    /// 技能音效
    /// </summary>
    public AudioClip skillClip;

    /// <summary>
    /// 伤害性技能的技能伤害
    /// </summary>
    public int damage;

    /// <summary>
    /// 恢复性技能恢复的生命值
    /// </summary>
    public int healHP;

    /// <summary>
    /// 恢复性技能恢复的mp
    /// </summary>
    public int healMP;

    /// <summary>
    /// buff技能增加的攻击力
    /// </summary>
    public int attackBuff;

    /// <summary>
    /// buff技能增加的防御力
    /// </summary>
    public int defBuff;

    /// <summary>
    ///攻击速度buff 
    /// </summary>
    public int attackSpeedBuff;

    /// <summary>
    /// 属性技能增加的暴击几率
    /// </summary>
    public int criticalAttribute;

    /// <summary>
    /// 属性技能增加的闪避
    /// </summary>
    public int evadeAttribute;

    /// <summary>
    /// 属性技能增加的力量
    /// </summary>
    public int powerAttribute;

    /// <summary>
    /// 属性技能增加的力量
    /// </summary>
    public int agilityAttribute;

    /// <summary>
    /// 属性技能增加的力量
    /// </summary>
    public int mentalityAttribute;

    /// <summary>
    /// 使用技能的消耗
    /// </summary>
    public int skillConsume;

    /// <summary>
    /// 动作名称 用来触发动作
    /// </summary>
    public string animatorName;

    /// <summary>
    /// 判断自己这个技能是不是cd中。
    /// </summary>
    public bool isCd;

    /// <summary>
    /// 技能的释放距离
    /// </summary>
    public int skillDistance;

    /// <summary>
    /// 开始计算cd条
    /// </summary>
    public bool cdSliderStart;

    /// <summary>
    /// 计算cd时间用
    /// </summary>
    public float cd_time = 0;

    #endregion

    #region 7种技能类型单例方法
    /// <summary>
    /// 单体伤害技能
    /// </summary>
    public static Skill MonomerDamageSkill(string _name, int _id, string _info,int _cd,string _effectPath , string _sprPath,string _skillClipPath,int _damage,int _skillConsume,string _animatorName,int _skillDistance)
    {
        Skill skill = new Skill();
        skill.skillType = SkillType.MonomerDamage;
        skill.name = _name;
        skill.id = _id;
        skill.info = _info;
        skill.level = 0;
        skill.cd = _cd;
        skill.skillEffect = Resources.Load<GameObject>(_effectPath);
        skill.skillSpr = Resources.Load<Sprite>(_sprPath);
        skill.skillClip = Resources.Load<AudioClip>(_skillClipPath);
        skill.damage = _damage;
        skill.skillConsume = _skillConsume;
        skill.animatorName = _animatorName;
        skill.skillDistance = _skillDistance;
        skill.isCd = false;
        return skill;
    }

    /// <summary>
    /// 单体治疗技能
    /// </summary>
    public static Skill MonomerTreatment(string _name, int _id, string _info, int _cd, string _effectPath, string _sprPath, string _skillClipPath, int _healHP,int _healMP ,int _skillConsume, string _animatorName, int _skillDistance)
    {
        Skill skill = new Skill();
        skill.skillType = SkillType.MonomerTreatment;
        skill.name = _name;
        skill.id = _id;
        skill.info = _info;
        skill.level = 0;
        skill.cd = _cd;
        skill.skillEffect = Resources.Load<GameObject>(_effectPath);
        skill.skillSpr = Resources.Load<Sprite>(_sprPath);
        skill.skillClip = Resources.Load<AudioClip>(_skillClipPath);
        skill.healHP = _healHP;
        skill.healMP = _healMP;
        skill.skillConsume = _skillConsume;
        skill.animatorName = _animatorName;
        skill.skillDistance = _skillDistance;
        skill.isCd = false;
        return skill;
    }

    /// <summary>
    /// 单体buff技能
    /// </summary>
    public static Skill MonomerBuffSkill(string _name, int _id, string _info, int _cd, string _effectPath, string _sprPath, string _skillClipPath, int _healHP, int _healMP, int _attackBuff, int _defBuff,int _attackSpeedBuff, int _criticalAttribute, int _evadeAttribute, int _time, int _skillConsume, string _animatorName, int _skillDistance)
    {
        Skill skill = new Skill();
        skill.skillType = SkillType.MonomerBuff;
        skill.name = _name;
        skill.id = _id;
        skill.info = _info;
        skill.level = 0;
        skill.cd = _cd;
        skill.skillEffect = Resources.Load<GameObject>(_effectPath);
        skill.skillSpr = Resources.Load<Sprite>(_sprPath);
        skill.skillClip = Resources.Load<AudioClip>(_skillClipPath);
        skill.healHP = _healHP;
        skill.healMP = _healMP;
        skill.attackBuff = _attackBuff;
        skill.defBuff = _defBuff;
        skill.attackSpeedBuff = _attackSpeedBuff;
        skill.criticalAttribute = _criticalAttribute;
        skill.evadeAttribute = _evadeAttribute;
        skill.time = _time;
        skill.skillConsume = _skillConsume;
        skill.animatorName = _animatorName;
        skill.skillDistance = _skillDistance;
        skill.isCd = false;
        return skill;
    }

    /// <summary>
    /// 群体伤害技能
    /// </summary>
    public static Skill GroupDamageSkill(string _name, int _id, string _info, int _cd, string _effectPath, string _sprPath, string _skillClipPath, int _damage, int _time, int _range, int _skillConsume , string _animatorName, int _skillDistance)
    {
        Skill skill = new Skill();
        skill.skillType = SkillType.GroupDamage;
        skill.name = _name;
        skill.id = _id;
        skill.info = _info;
        skill.level = 0;
        skill.cd = _cd;
        skill.skillEffect = Resources.Load<GameObject>(_effectPath);
        skill.skillSpr = Resources.Load<Sprite>(_sprPath);
        skill.skillClip = Resources.Load<AudioClip>(_skillClipPath);
        skill.damage = _damage;
        skill.time = _time;
        skill.range = _range;
        skill.skillConsume = _skillConsume;
        skill.animatorName = _animatorName;
        skill.skillDistance = _skillDistance;
        skill.isCd = false;
        return skill;
    }

    /// <summary>
    /// 群体治疗技能
    /// </summary>
    public static Skill GroupTherapySkill(string _name, int _id, string _info, int _cd, string _effectPath, string _sprPath, string _skillClipPath, int _healHP, int _healMP, int _range, int _skillConsume,string _animatorName, int _skillDistance)
    {
        Skill skill = new Skill();
        skill.skillType = SkillType.GroupTherapy;
        skill.name = _name;
        skill.id = _id;
        skill.info = _info;
        skill.level = 0;
        skill.cd = _cd;
        skill.skillEffect = Resources.Load<GameObject>(_effectPath);
        skill.skillSpr = Resources.Load<Sprite>(_sprPath);
        skill.skillClip = Resources.Load<AudioClip>(_skillClipPath);
        skill.healHP = _healHP;
        skill.healMP = _healMP;
        skill.range = _range;
        skill.skillConsume = _skillConsume;
        skill.animatorName = _animatorName;
        skill.skillDistance = _skillDistance;
        skill.isCd = false;
        return skill;
    }

    /// <summary>
    /// 群体buff技能
    /// </summary>
    public static Skill GroupBuffSkill(string _name, int _id, string _info, int _cd, string _effectPath, string _sprPath, string _skillClipPath,int _healHP, int _healMP, int _attackBuff, int _defBuff, int _attackSpeedBuff, int _criticalAttribute, int _evadeAttribute, int _time, int _range, int _skillConsume, string _animatorName, int _skillDistance)
    {
        Skill skill = new Skill();
        skill.skillType = SkillType.GroupBuff;
        skill.name = _name;
        skill.id = _id;
        skill.info = _info;
        skill.level = 0;
        skill.cd = _cd;
        skill.skillEffect = Resources.Load<GameObject>(_effectPath);
        skill.skillSpr = Resources.Load<Sprite>(_sprPath);
        skill.skillClip = Resources.Load<AudioClip>(_skillClipPath);
        skill.healHP = _healHP;
        skill.healMP = _healMP;
        skill.attackBuff = _attackBuff;
        skill.defBuff = _defBuff;
        skill.attackSpeedBuff = _attackSpeedBuff;
        skill.criticalAttribute = _criticalAttribute;
        skill.evadeAttribute = _evadeAttribute;
        skill.time = _time;
        skill.range = _range;
        skill.skillConsume = _skillConsume;
        skill.animatorName = _animatorName;
        skill.skillDistance = _skillDistance;
        skill.isCd = false;
        return skill;
    }

    /// <summary>
    /// 添加属性技能
    /// </summary>
    public static Skill AddAttributeSkill(string _name, int _id, string _info, string _sprPath, int _criticalAttribute,int _evadeAttribute, int _powerAttribute, int _agilityAttribute,int _mentalityAttribute)
    {
        Skill skill = new Skill();
        skill.skillType = SkillType.AddAttribute;
        skill.name = _name;
        skill.id = _id;
        skill.info = _info;
        skill.level = 0;
        skill.skillSpr = Resources.Load<Sprite>(_sprPath);
        skill.criticalAttribute = _criticalAttribute;
        skill.evadeAttribute = _evadeAttribute;
        skill.powerAttribute = _powerAttribute;
        skill.agilityAttribute = _agilityAttribute;
        skill.mentalityAttribute = _mentalityAttribute;
        skill.isCd = false;
        return skill;
    }

    public Skill Coby(Skill otrSkill)
    {
        Skill skill = new Skill();
        skill.skillType = otrSkill.skillType;
        skill.name = otrSkill.name;
        skill.id = otrSkill.id;
        skill.info = otrSkill.info;
        skill.level = 0;
        skill.skillSpr =otrSkill.skillSpr;
        skill.criticalAttribute = otrSkill.criticalAttribute;
        skill.evadeAttribute = otrSkill.evadeAttribute;
        skill.powerAttribute = otrSkill.powerAttribute;
        skill.agilityAttribute = otrSkill.agilityAttribute;
        skill.mentalityAttribute = otrSkill.mentalityAttribute;
        skill.isCd = false;
        return skill;
    }
    #endregion
}
