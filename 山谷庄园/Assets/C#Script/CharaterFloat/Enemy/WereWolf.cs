using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WereWolf : WolfCha {
    #region 继承的方法

    public override void AddInformation()
    {
        wolf = new Charater();
        wolf.name = "黑狼王";
        wolf.id = 6;
        wolf.level = 30;

        wolf.power = 14 + wolf.level * 5;
        wolf.agility = 10 + wolf.level * 3;
        wolf.mentality = 2 + wolf.level * 1;

        wolf.basicsHp = 1000;
        wolf.basicsMp = 100;
        wolf.basicsAttack = 150;
        wolf.basicsDef = 50;
        wolf.basicsCritical = 8;
        wolf.basicsEvade = 8;
        wolf.basicsAttackSpeed = 2;

        wolf.isAlive = true;
        wolf.attackDistance = 1.5f;
        wolf.money = 500;

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
        initialRotation = transform.rotation;
    }

    public override void Update()
    {
        AnimatorControl();
        base.Update();
    }


    public override void AnimatorControl()
    {
        animator.SetFloat("WWRunBlend", agent.velocity.magnitude);
        animator.SetBool("WWIsAlive", wolf.isAlive);
    }

    public override bool FindAttackTarget()
    {
        GameObject obj = GameObject.Find("WolfBigBoss");
        if (obj.GetComponent<WolfCha>().wolf.isAlive==false)
        {
            currenAttacktarget = FindObjectOfType<PlayerControl>().gameObject;
            if (currenAttacktarget.GetComponent<PlayerCharater>().player.isAlive==false)  //如果这个目标已经挂了
            {
                PlayerCharater [] players = FindObjectsOfType<PlayerCharater>();
                for (int i = 0; i < players.Length; i++)   //那么就找到当前还存活着的玩家角色。
                {
                    if (players[i].player.isAlive)
                    {
                        currenAttacktarget = players[i].gameObject;
                    }
                }
            }
            if (currenAttacktarget != null)
            {
                return true;
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
                animator.SetTrigger("WWAttack");
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

    /// <summary>
    /// 野狼的攻击帧动画
    /// </summary>
    public void EventAttack()
    {
        if (currenAttacktarget != null)
        {
            if (currenAttacktarget.GetComponent<PlayerCharater>() != null)
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
    }

    public override void MonsterOnCriticalStrike()
    {
        animator.SetTrigger("ONCriticalStrike");
    }

    public override bool LoseTarget()
    {
        if (currenAttacktarget.GetComponent<PlayerCharater>().player.isAlive == false)  //如果当前距离起始点过远。 或者当前攻击目标的生命值小于等于0
        {
            Debug.Log(transform.name + "没有攻击目标了");
            return true;
        }
        return false;
    }
    public override void AddItem()
    {
        wolfBackPack = new List<Item>();
        for (int i = 0; i < 6; i++)
        {
            wolfBackPack.Add(ItemManager.Instance().EQItemsShop[Random.Range(20, 28)]);
        }

        for (int i = 0; i < wolfBackPack.Count; i++)
        {
            WolfAddItemAttribute(wolfBackPack[i]);
        }
    }

    #endregion

}
