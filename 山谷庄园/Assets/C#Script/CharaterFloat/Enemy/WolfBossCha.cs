using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WolfBossCha : WolfCha {
    /// <summary>
    /// 最大追击距离
    /// </summary>
    public float maxChaseDistance;

    #region 继承的方法

    public override void AddInformation()
    {
        wolf = new Charater();
        wolf.name = "红狼王";
        wolf.id = 5;
        wolf.level = 20;

        wolf.power = 14 + wolf.level * 5;
        wolf.agility = 10 + wolf.level * 3;
        wolf.mentality = 2 + wolf.level * 1;

        wolf.basicsHp = 1000;
        wolf.basicsMp = 100;
        wolf.basicsAttack = 120;
        wolf.basicsDef = 40;
        wolf.basicsCritical = 8;
        wolf.basicsEvade = 8;
        wolf.basicsAttackSpeed = 2;

        


        wolf.isAlive = true;
        maxChaseDistance = 8f;
        wolf.attackDistance = 1.5f;
        wolf.money = 248;

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
        animator.SetFloat("WSRunBlend", agent.velocity.magnitude);
        animator.SetBool("WSIsAlive", wolf.isAlive);
    }

    public override bool FindAttackTarget()
    {
        if (!wolf.isAlive)
        {
            return false;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if ((transform.position - initialPos).magnitude < maxChaseDistance - 3f)
            {
                if (colliders[i].gameObject.GetComponent<PlayerCharater>() != null)
                {
                    if (colliders[i].gameObject.GetComponent<PlayerCharater>().player.isAlive)
                    {

                        currenAttacktarget = colliders[i].gameObject;
                        return true;
                    }
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
                if (MonsterCriticalStrike(wolf))
                {
                    animator.SetTrigger("WSAttackC");
                    playerCha.player.hp -= wolf.attack * 2 - playerCha.player.def;
                    GameObject obj = Instantiate(damageText, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    obj.AddComponent<TextDamage>();
                    obj.GetComponent<TextDamage>().GetInfo("暴击", Color.red);
                }
                else
                {
                    animator.SetTrigger("WSAttack");
                    playerCha.player.hp -= wolf.attack - playerCha.player.def;
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

    public void OnWolfBossAnimationEvent_Attack()
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

    public void OnWolfBossAnimationEvent_AttackC()
    {
        if (currenAttacktarget != null)
        {
            if (currenAttacktarget.GetComponent<PlayerCharater>() != null)
            {
                currenAttacktarget.GetComponent<PlayerCharater>().PlayerOnCriticalStrike();
                AudioSource.PlayClipAtPoint(AttackClip, currenAttacktarget.transform.position);
                GameObject obj = Instantiate(AttackEffect, currenAttacktarget.transform.position, Quaternion.identity);
                Destroy(obj, 0.4f);
            }
        }
    }

    public override void MonsterOnHit()
    {
        renderer.material = redMaterial;
        Invoke("ChangeMaterial", 0.5f);
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
