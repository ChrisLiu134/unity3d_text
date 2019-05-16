using System.Collections;
using System.Collections.Generic;


public class Charater  {

    #region 名字， id ，生命值，最大生命值，魔法值，最大魔法值，经验值，最大经验值，攻击力，防御力,攻击速度,移动速度，转身速度,空构造。 

    /// <summary>
    /// 角色的名字
    /// </summary>
    public string name;

    /// <summary>
    /// 角色id
    /// </summary>
    public int id;

    /// <summary>
    /// 等级
    /// </summary>
    public int level;

    /// <summary>
    /// 角色力量
    /// </summary>
    public int power;

    /// <summary>
    /// 敏捷
    /// </summary>
    public int agility;

    /// <summary>
    /// 智力
    /// </summary>
    public int mentality;

    /// <summary>
    /// 角色生命值
    /// </summary>
    public int hp;
    public int basicsHp;

    /// <summary>
    /// 最大生命值
    /// </summary>
    public int maxHp;

    /// <summary>
    /// 角色魔法值
    /// </summary>
    public int mp;
    public int basicsMp;

    /// <summary>
    /// 最大魔法值
    /// </summary>
    public int maxMp;

    /// <summary>
    /// 角色经验值
    /// </summary>
    public int exp;

    /// <summary>
    /// 最大经验值
    /// </summary>
    public int maxExp;

    /// <summary>
    /// 角色攻击力
    /// </summary>
    public int attack;
    public int basicsAttack;
    /// <summary>
    /// 角色防御力
    /// </summary>
    public int def;
    public int basicsDef;

    /// <summary>
    /// 攻击速度
    /// </summary>
    public float attackSpeed;
    public float basicsAttackSpeed;

    /// <summary>
    /// 闪避系数，我方的闪避影响对面的丢失概率
    /// </summary>
    public float evade;
    public float basicsEvade;

    /// <summary>
    /// 暴击系数
    /// </summary>
    public float critical ;
    public float basicsCritical;

    /// <summary>
    /// 是否存活
    /// </summary>
    public bool isAlive;

    /// <summary>
    /// 攻击距离
    /// </summary>
    public float attackDistance;

    /// <summary>
    /// 金钱
    /// </summary>
    public int money;

    /// <summary>
    /// 技能点
    /// </summary>
    public int skillPoints = 1;
    #endregion

}
