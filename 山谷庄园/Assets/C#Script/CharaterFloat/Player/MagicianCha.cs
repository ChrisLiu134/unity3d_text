using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicianCha : PlayerCharater
{


    protected override void Awake()
    {
        AddInformation();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        renderer = GetComponentInChildren<Renderer>();
    }

    protected override void Start()
    {

    }

    protected override void Update()
    {
        AnimatorControl();
        base.Update();
    }

    #region 角色行为方法， 继承和实现的角色行为接口的方法。

    /// <summary>
    /// 设置这个角色的属性信息
    /// </summary>
    public override void AddInformation()
    {
        player = new Charater();
        player.name = "小仙女";
        player.id = 2;
        player.level = 1;
        player.skillPoints = 1;
        //属性伴随等级的成长系统
        player.power = 11;
        player.agility = 16;
        player.mentality = 21;
        //

        //基础属性
        player.basicsHp = 100;
        player.basicsMp = 200;
        player.basicsAttack = 30;
        player.basicsDef = 4;
        player.basicsCritical = 5;
        player.basicsEvade = 8;
        player.basicsAttackSpeed = 2;
        //
        //
        player.exp = 0;
        player.maxExp = 68;
        //

        //玩家数据 力量敏捷智力等属性加上相应的基础属性，  基础属性是用来装备增加的
        RefreshAttribute();

        //初始设置玩家金币为1000  状态为存活状态。
        player.isAlive = true;
        player.attackDistance = 4f;
        player.money = 1000;

        playerSkillState = new List<SkillState>();
    }

    /// <summary>
    /// 动作控制器
    /// </summary>
    public override void AnimatorControl()
    {
        animator.SetFloat("MGRunBlend", agent.velocity.magnitude);
        animator.SetBool("MGIsAlive", player.isAlive);
    }

    /// <summary>
    /// 移动,如果目标是怪物就发动攻击
    /// </summary>
    public override void Moving(Vector3 pos)
    {
        agent.destination = pos;
    }

    /// <summary>
    /// 查看是否靠近攻击目标了
    /// </summary>
    public override bool CloseToTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, player.attackDistance);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (currenBattleTraget != null)   //如果当前选中了攻击的目标
            {
                if (colliders[i].gameObject == currenBattleTraget)   //如果当前 目标在自己的攻击范围内
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 发动攻击
    /// </summary>
    float temp_time1 = 0;
    public override void PlayerAttack(WolfCha monster)
    {
        temp_time1 += Time.deltaTime;
        if (temp_time1 > player.attackSpeed)
        {
            temp_time1 = 0;
            if (monster.Evade(monster.wolf))  //敌人通过它的闪避系数计算敌人的闪避是否成功    
            {
                if (PlayerCriticalStrike(player))  //如果暴击了
                {
                    animator.SetTrigger("MGAttackC");
                    monster.wolf.hp -= (player.attack*3) - monster.wolf.def;
                    GameObject obj = Instantiate(damageText, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    obj.AddComponent<TextDamage>();
                    obj.GetComponent<TextDamage>().GetInfo("暴击", Color.red);
                }
                else
                {
                    animator.SetTrigger("MGAttack");
                    monster.wolf.hp -= player.attack - monster.wolf.def;
                }
            }
            else
            {
                GameObject obj = Instantiate(damageText, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                obj.AddComponent<TextDamage>();
                obj.GetComponent<TextDamage>().GetInfo("miss", Color.white);
            }
        }
    }

    /// <summary>
    /// 技能动作帧动画
    /// </summary>
    public void OnMagicianAnimationEvent_MagicianSkill()
    {
        if (currenBattleTraget != null)
        {
            currenBattleTraget.GetComponent<WolfCha>().MonsterOnCriticalStrike();
            AudioSource.PlayClipAtPoint(AttackClip, currenBattleTraget.transform.position);
            GameObject obj = Instantiate(AttackEffect, currenBattleTraget.transform.position, Quaternion.identity);
            Destroy(obj, 0.4f);
        }
    }

    /// <summary>
    /// 暴击动作帧动画
    /// </summary>
    public void OnMagicianAnimationEvent_AttackC()
    {
        if (currenBattleTraget != null)
        {
            currenBattleTraget.GetComponent<WolfCha>().MonsterOnCriticalStrike();
            AudioSource.PlayClipAtPoint(AttackClip, currenBattleTraget.transform.position);
            GameObject obj = Instantiate(AttackEffect, currenBattleTraget.transform.position, Quaternion.identity);
            Destroy(obj, 0.4f);
        }
    }

    /// <summary>
    /// 普攻动作帧动画
    /// </summary>
    public void OnMagicianAnimationEvent_Attack()
    {
        if (currenBattleTraget != null)
        {
            currenBattleTraget.GetComponent<WolfCha>().MonsterOnHit();
            AudioSource.PlayClipAtPoint(AttackClip, currenBattleTraget.transform.position);
            GameObject obj = Instantiate(AttackEffect, currenBattleTraget.transform.position, Quaternion.identity);
            Destroy(obj, 0.4f);
        }
    }


    /// <summary>
    /// 被攻击
    /// </summary>
    public override void PlayerOnHit()
    { 
        renderer.material = redMaterial;
        Invoke("ChangeMaterial", 0.5f);
    }

    public override void PlayerOnCriticalStrike()
    {
        animator.SetTrigger("OnCriticalStrike");
        renderer.material = redMaterial;
        Invoke("ChangeMaterial", 0.5f);
    }

    /// <summary>
    /// 角色升级
    /// </summary>
    public override void UpGrade()
    {
        player.skillPoints += 1;
        player.level += 1;
        player.maxExp += player.level * 42;
        player.power += 1;
        player.agility += 2;
        player.mentality += 4;
        GameObject obj = Instantiate(upGradeEffect, transform.position, Quaternion.identity);
        Destroy(obj, 0.4f);
        RefreshAttribute();  //刷新玩家的属性


    }

    #endregion

}
