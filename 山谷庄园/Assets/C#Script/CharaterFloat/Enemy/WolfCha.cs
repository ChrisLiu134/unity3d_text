using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfCha : MonoBehaviour {


    /// <summary>
    /// 角色对象
    /// </summary>
    public Charater wolf;

    /// <summary>
    /// 当前攻击的目标
    /// </summary>
    public GameObject currenAttacktarget;

    /// <summary>
    /// 动作组件
    /// </summary>
    public Animator animator;

    /// <summary>
    /// navmesh 组件
    /// </summary>
    public NavMeshAgent agent;

    /// <summary>
    /// 自己的起始位置
    /// </summary>
    public Vector3 initialPos;

    /// <summary>
    /// 初始方向
    /// </summary>
    public Quaternion initialRotation;

    /// <summary>
    /// 攻击特效
    /// </summary>
    public GameObject AttackEffect;

    //修改被攻击后的材质变化
    public Renderer renderer;
    public Material oldMaterial;
    public Material redMaterial;
    //

    public AudioClip AttackClip; //攻击音频
    public AudioClip deadClip;  //死亡音频
    public GameObject damageText; //攻击信息

    /// <summary>
    /// 狼崽子门身上的装备
    /// </summary>
    public List<Item> wolfBackPack;

    /// <summary>
    /// 场景内的相机
    /// </summary>
    Transform cameraTrans;

    /// <summary>
    /// 怪物身上的状态储存点
    /// </summary>
    public List<SkillState> monsterSkillState;

    #region 父类给子类定义的方法 和一些父类已经写好的方法， start ，update ， 动作添加，添加信息， 巡逻---
    //寻找攻击目标，移动和朝向攻击目标，怪物发动攻击，丢失攻击的目标，被攻击，闪避， 死亡， 升级，持续的恢复，更换材质，
    //暴击计算， 被暴击攻击， 如果靠近玩家就启动行为树，

    public virtual void Start()
    {
       
    }

    public virtual void Update()
    {
        Recover();
        SettlementStatus();
    }

    public virtual void AddInformation()
    {
        //添加怪物信息 不同子类脚本添加不同信息
    }

    public virtual void AnimatorControl()
    {
        //添加所有的动作
    }

    public virtual void Patrol()
    {
        //怪物从一个点移动到另外一个点，通过怪物控制器控制？
    }

    /// <summary>
    /// 怪物搜索攻击目标 小狼是如果被攻击则有了攻击的目标
    /// </summary>
    public virtual bool FindAttackTarget()
    {
        return false;
    }


    /// <summary>
    /// 移动和朝向目标
    /// </summary>
    public void MoveAndFaceToTarget()
    {
        if (wolf.isAlive)
        {
            if (currenAttacktarget != null)
            {
                agent.stoppingDistance = wolf.attackDistance;
                agent.destination = currenAttacktarget.transform.position;
                transform.LookAt(currenAttacktarget.transform);
            }
        }
        
    }


    public virtual void MonsterAttack(PlayerCharater player)
    {
        //怪物的攻击方式， 扣血方式
    }

    public virtual void MonsterOnHit()
    {
        //怪物被攻击， 设置怪物的被攻击动作
    }

    /// <summary>
    /// 闪避系统
    /// </summary>
    /// <param name="cha"></param>
    /// <returns></returns>
    public bool Evade(Charater cha)
    {
        int n = Random.Range(0, 200);
        if (cha.evade < n)
        {
            return true;
        }

        return false;
    }

    public virtual bool LoseTarget()
    {
        return false;
    }

    /// <summary>
    /// 返回起始点
    /// </summary>
    public bool Back()
    {
        agent.destination = initialPos;
        if ((agent.destination.x - initialPos.x <= 1.5f)
             && (agent.destination.y - initialPos.y <= 1.5f)
             && (agent.destination.z - initialPos.z <= 1.5f))
        {
            transform.rotation = initialRotation;
            return true;
        }
        return false;
    }

    public void dead()
    {
        currenAttacktarget = null;
        if (!wolf.isAlive)
        {
            return;
        }
        PlayerCharater[] players = FindObjectsOfType<PlayerCharater>();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetExp(wolf);
            players[i].player.money += wolf.money;
        }
        AudioSource.PlayClipAtPoint(deadClip, transform.position);
        wolf.isAlive = false;
        agent.enabled = false;
        gameObject.layer = 12;
    }

    public virtual void UpGrade()
    {
       //小狼和普通狼人会一直升级到10级
    }
    /// <summary>
    /// 死亡后复活
    /// </summary>
    public void Relive()
    {
        gameObject.layer = 9;
        agent.enabled = true;
        transform.position = initialPos;
        agent.ResetPath();
        wolf.hp = wolf.maxHp;
        wolf.mp = wolf.maxHp;
        wolf.isAlive = true;
        Debug.Log(transform.name + "复活了");
    }

    /// <summary>
    /// 每4秒恢复一定的生命值和魔法值,并且当怪物角色的血量和魔法值超过上限的时候控制住
    /// </summary>
    float temp_Time = 0; //这个方法用到的计时器
    public  void  Recover()
    {
        if (wolf.isAlive==false)
        {
            return;
        }
        if (wolf.hp >= wolf.maxHp)
        {
            wolf.hp = wolf.maxHp;
        }
        if (wolf.mp >= wolf.maxMp)
        {
            wolf.mp = wolf.maxMp;
        }
        temp_Time += Time.deltaTime;
        if (temp_Time > 4)
        {
            wolf.hp += wolf.power / 10;
            wolf.mp += wolf.mentality / 10;
            temp_Time = 0;
        }
    }

    /// <summary>
    /// 暴击计算
    /// </summary>
    public bool MonsterCriticalStrike(Charater cha)
    {
        int n = Random.Range(0, 200);
        if (cha.critical > n)
        {
            return true;   //闪避失败  就返回true
        }
        return false;
    }

    /// <summary>
    /// 被暴击攻击
    /// </summary>
    public virtual void MonsterOnCriticalStrike()
    {

    }

    /// <summary>
    /// 接近了玩家，就开始执行行为树的各个行为， 这是一个省性能的操作。  远离玩家了，就不执行行为树的各种操作
    /// </summary>
    public bool ClosePlayer()
    {
        //如果自己是小狼
        if (gameObject.name=="WolfBaby")
        {
            //当自己处于相机视野内时  就需要开启行为树。 因为小狼要在玩家看到时候要在巡逻
            cameraTrans = Camera.main.transform;
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            Vector3 dir = (transform.position - cameraTrans.position).normalized;
            float dot = Vector3.Dot(cameraTrans.forward, dir);
            if ( dot>0 && viewPos.x>0 && viewPos.x<1 && viewPos.y>0&&viewPos.y<1)
            {
                return true;
            }
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if ((players[i].transform.position-transform.position).magnitude<8f)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 跟换材质
    /// </summary>
    public void ChangeMaterial()
    {
        renderer.material = oldMaterial;
    }


    /// <summary>
    /// 根据力量敏捷值刷新属性
    /// </summary>
    public virtual void RefreshAttribute()
    {
        wolf.hp = wolf.basicsHp + wolf.power * 19;
        wolf.maxHp = wolf.hp;
        wolf.mp = wolf.basicsMp + wolf.mentality * 13;
        wolf.maxMp = wolf.mp;
        wolf.attack = wolf.basicsAttack + wolf.power;
        wolf.def = wolf.basicsDef + wolf.power;
        wolf.attackSpeed = wolf.basicsAttackSpeed - (float)wolf.agility / 200;
        wolf.evade = wolf.basicsEvade + wolf.agility / 10;
        wolf.critical = wolf.basicsCritical + wolf.agility / 8;
    }

    /// <summary>
    /// 添加buff时 计算属性
    /// </summary>
    public void CountAttribute()
    {
        wolf.maxHp = wolf.basicsHp + wolf.power * 19;
        wolf.maxMp = wolf.basicsMp + wolf.mentality * 13;
        wolf.attack = wolf.basicsAttack + wolf.power;
        wolf.def = wolf.basicsDef + wolf.power;
        wolf.attackSpeed = wolf.basicsAttackSpeed - (float)wolf.agility / 200;
        wolf.evade = wolf.basicsEvade + wolf.agility / 10;
        wolf.critical = wolf.basicsCritical + wolf.agility / 8;
    }

    /// <summary>
    /// 给怪物添加道具
    /// </summary>
    public virtual void AddItem()
    {

    }

    /// <summary>
    /// 玩家丢弃装备
    /// </summary>
    /// <param name="item"></param>
    public void WolfAddItemAttribute(Item item)
    {
        if (item.itemType == Item.ItemType.EQ)   //丢弃了装备，就减掉这个装备带来的各种属性      
        {
            wolf.power += item.power;
            wolf.agility += item.agility;
            wolf.mentality += item.mentality;
            wolf.basicsAttack += item.attack;
            wolf.basicsDef += item.def;
            wolf.basicsAttackSpeed -= item.attackSpeed/200;
            wolf.basicsEvade += item.evade;
            wolf.basicsCritical += item.critical;
            wolf.basicsHp += item.hp;
            wolf.basicsMp += item.mp;
            wolf.maxHp += item.hp;
            wolf.maxMp += item.mp;
        }
    }

    /// <summary>
    /// 结算状态
    /// </summary>
    public void SettlementStatus()
    {
        if (monsterSkillState.Count == 0)
        {
            return;
        }
        for (int i = 0; i < monsterSkillState.Count; i++)
        {
            monsterSkillState[i].cd_time += Time.deltaTime;
            if (monsterSkillState[i].cd_time > monsterSkillState[i].time)
            {
                //删除状态
                SkillManager.Instance().RemoveState(monsterSkillState[i], wolf);
                monsterSkillState[i].haveState = false;
                monsterSkillState.RemoveAt(i);
                RefreshAttribute();
            }
        }
    }

    #endregion
}
