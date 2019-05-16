using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 剑客角色类
/// </summary>
public class SwordmanCha :PlayerCharater {

    protected override void Awake()
    {

    }

    protected override void Start()
    {
        AddInformation();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        renderer = GetComponentInChildren<Renderer>();
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
        player.name = "敏敏酱";
        player.id = 1;
        player.level = 1;
        player.skillPoints = 1;
        //属性伴随等级的成长系统
        player.power = 16;
        player.agility = 14;
        player.mentality = 5;
        //

        //基础属性
        player.basicsHp = 200;
        player.basicsMp = 100;
        player.basicsAttack = 30;
        player.basicsDef = 8;
        player.basicsCritical = 8;
        player.basicsEvade = 5;
        player.basicsAttackSpeed = 2f;
        //
        //
        player.exp = 0;
        player.maxExp = 68;
        //

        //玩家数据 力量敏捷智力等属性加上相应的基础属性，  基础属性是用来装备增加的
        RefreshAttribute();

        //初始设置玩家金币为1000  状态为存活状态。
        player.isAlive = true;
        player.attackDistance = 1.5f;
        player.money = 1000;

        playerSkillState = new List<SkillState>();
    }

    /// <summary>
    /// 动作控制器
    /// </summary>
    public override void AnimatorControl()
    {
        animator.SetFloat("SWRunBlend", agent.velocity.magnitude);
        animator.SetBool("SWIsAlive", player.isAlive);
    }

    public override void Moving(Vector3 pos)
    {
        agent.destination = pos;
    }

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

    float temp_time1 = 0;
    public override void PlayerAttack( WolfCha monster)
    {
        temp_time1 += Time.deltaTime;
        if (temp_time1>player.attackSpeed)
        {
            temp_time1 = 0;
           
            if (monster.Evade(monster.wolf))
            {
                if (PlayerCriticalStrike(player))
                {
                    animator.SetTrigger("SWAttackC");
                    monster.wolf.hp -= (player.attack * 3) - monster.wolf.def;
                    GameObject obj = Instantiate(damageText, transform.position + new Vector3(0,1,0), Quaternion.identity);
                    obj.AddComponent<TextDamage>();
                    obj.GetComponent<TextDamage>().GetInfo("暴击", Color.red);
                }
                else
                {
                    animator.SetTrigger("SWAttack");
                    monster.wolf.hp -= player.attack - monster.wolf.def;
                }
               
            }
            else
            {
                GameObject obj = Instantiate(damageText, transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
                obj.AddComponent<TextDamage>();
                obj.GetComponent<TextDamage>().GetInfo("miss", Color.white);
            }
        }
    }

    /// <summary>
    /// 攻击动画的帧事件方法 
    /// </summary>
    public void OnSwordmanAnimationEvent_Attack() 
    {
        if (currenBattleTraget!=null)
        {
            currenBattleTraget.GetComponent<WolfCha>().MonsterOnHit();
            AudioSource.PlayClipAtPoint(AttackClip, currenBattleTraget.transform.position);
            GameObject obj = Instantiate(AttackEffect, currenBattleTraget.transform.position, Quaternion.identity);
            Destroy(obj, 0.4f);
        }
    }

    public void OnSwordmanAnimationEvent_AttackC()
    {
        if (currenBattleTraget != null)
        {
            currenBattleTraget.GetComponent<WolfCha>().MonsterOnCriticalStrike();
            AudioSource.PlayClipAtPoint(AttackClip, currenBattleTraget.transform.position);
            GameObject obj = Instantiate(AttackEffect, currenBattleTraget.transform.position, Quaternion.identity);
            Destroy(obj, 0.4f);
        }
    }


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
    /// 使用技能攻击的帧动画
    /// </summary>
    public void OnSwordmanAnimationEvent_SwordmanSkill()
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
    /// 升级
    /// </summary>
    public override void UpGrade()
    {
        player.skillPoints += 1;
        player.level += 1;
        player.maxExp += player.level * 42;
        //没升一级 增加 4点力量， 2点敏捷， 1点智力
        player.power +=  4;
        player.agility +=  2;
        player.mentality += 1;
        //没升一级，增加各项属性
        RefreshAttribute();
        GameObject obj = Instantiate(upGradeEffect, transform.position, Quaternion.identity);
        Destroy(obj, 0.4f);
          
    }

    #endregion


}
