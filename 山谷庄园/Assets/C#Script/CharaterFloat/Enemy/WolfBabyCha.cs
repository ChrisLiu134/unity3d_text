using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfBabyCha:WolfCha {

    /// <summary>
    /// 是否处于巡逻状态
    /// </summary>
    bool onPatrol ;

    /// <summary>
    /// 巡逻离起始点的最大距离
    /// </summary>
    public float maxPatrolDistance;

    /// <summary>
    /// 最大追击距离
    /// </summary>
    public float maxChaseDistance;


    #region 继承的方法

    public override void AddInformation()
    {
        wolf = new Charater();
        wolf.name = "狼宝宝";
        wolf.id = 3;
        wolf.level = 3;

        wolf.power = 14 + wolf.level * 3;
        wolf.agility = 10 + wolf.level * 2;
        wolf.mentality = 2 + wolf.level * 1;

        wolf.basicsHp = 300;
        wolf.basicsMp = 100;
        wolf.basicsAttack = 40;
        wolf.basicsDef = 8;
        wolf.basicsCritical = 8;
        wolf.basicsEvade = 8;
        wolf.basicsAttackSpeed = 2;

       

        wolf.isAlive = true;
        wolf.attackDistance = 1.5f;
        wolf.money = 74;
        maxPatrolDistance = 6f;
        maxChaseDistance = 8f;
        //给怪物添加装备道具
        AddItem();

        //刷新属性
        RefreshAttribute();

        monsterSkillState = new List<SkillState>();
    }

    public override void Start()
    {
        AddInformation();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponentInChildren<Renderer>();
        initialPos = transform.position;
        onPatrol = false;
    }

    public override void Update()
    {
        AnimatorControl();
        base.Update();
        UpGrade();
    }


    public override void AnimatorControl()
    {
        animator.SetFloat("WBRunBlend", agent.velocity.magnitude);
        animator.SetBool("WBIsAlive", wolf.isAlive);
    }

    public override void Patrol()
    {
        agent.ResetPath();
        if (!onPatrol )
        {
            Vector3 nextPos = new Vector3(Random.Range(-4.5f,4.5f), 0, Random.Range(-4.5f, 4.5f)) + initialPos;
            if ((nextPos - transform.position).magnitude < maxPatrolDistance)
            {
                agent.destination = nextPos;
                onPatrol = true;
            }
        }
        if (onPatrol)
        {
            if ((agent.destination.x - agent.nextPosition.x <= 0.05f)
              && (agent.destination.y - agent.nextPosition.y <= 0.05f)
              && (agent.destination.z - agent.nextPosition.z <= 0.05f))
            {
                onPatrol = false;
            }
        }
       
    }

    public override bool FindAttackTarget()
    {
        if (!wolf.isAlive)
        {
            return false;
        }
        //这种小狼角色， 只有在受到攻击之后， 才会反击攻击它的目标
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<PlayerCharater>() != null)
            {
                if (colliders[i].GetComponent<PlayerCharater>().currenBattleTraget == gameObject)
                {
                    currenAttacktarget = colliders[i].gameObject;
                    return true;
                }

            }
        }
        return false;
    }

    float temp_time1 = 0;  //计时器
    public override void MonsterAttack(PlayerCharater playerCha)
    {
        temp_time1 += Time.deltaTime;
        if (temp_time1 > wolf.attackSpeed)
        {
            temp_time1 = 0;
            if (playerCha.Evade(playerCha.player))  //计算敌人闪避是否成功    
            {
                animator.SetTrigger("WBAttack");
                playerCha.player.hp -= wolf.attack - playerCha.player.def;
               
            }
            else
            {
                GameObject obj = Instantiate(damageText, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                obj.AddComponent<TextDamage>();
                obj.GetComponent<TextDamage>().GetInfo("miss", Color.white);
            }
        }
    }

    public void OnWolfBabyAnimationEvent_Attack()
    {
        if (currenAttacktarget!=null)
        {
            if (currenAttacktarget.GetComponent<PlayerCharater>()!=null)
            {
                currenAttacktarget.GetComponent<PlayerCharater>().PlayerOnHit();
                AudioSource.PlayClipAtPoint(AttackClip, currenAttacktarget.transform.position);
                GameObject obj = Instantiate(AttackEffect, currenAttacktarget.transform.position, Quaternion.identity);
                Destroy(obj, 0.4f);
            }
        }
    }

    public override void MonsterOnHit()
    {
        renderer.material = redMaterial;
        Invoke("ChangeMaterial",0.5f);
    }

    public override void MonsterOnCriticalStrike()
    {
        animator.SetTrigger("OnCriticalStrike");
        renderer.material = redMaterial;
        Invoke("ChangeMaterial", 0.5f);
    }


    public override bool LoseTarget()
    {
        if ((transform.position - initialPos).magnitude > maxChaseDistance || currenAttacktarget.GetComponent<PlayerCharater>().player.isAlive == false)  //如果当前距离起始点过远。 或者当前攻击目标的生命值小于等于0
        {
            Debug.Log(transform.name+"没有攻击目标了");
            return true;
        }
        return false;
    }

    /// <summary>
    /// 小狼有一个每10分钟升一级的系统
    /// </summary>
    float time_1;
    public override void UpGrade()
    {
        if (wolf.level >= 10)
        {
            return;
        }
        if (Time.time - time_1 > 120)
        {
            time_1 = Time.time;
            wolf.power += 4;
            wolf.agility += 3;
            wolf.mentality += 1;
            RefreshAttribute();
        }
    }

    public override void AddItem()
    {
        wolfBackPack = new List<Item>();
        for (int i = 0; i < 2; i++)
        {
            wolfBackPack.Add(ItemManager.Instance().EQItemsShop[Random.Range(1, 10)]);
        }
        for (int i = 0; i < 4; i++)
        {
            wolfBackPack.Add(new Item());
        }

        for (int i = 0; i < wolfBackPack.Count; i++)
        {
            WolfAddItemAttribute(wolfBackPack[i]);
        }
    }

    #endregion
}
