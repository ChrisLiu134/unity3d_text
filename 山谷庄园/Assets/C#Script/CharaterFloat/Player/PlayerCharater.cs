using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharater : MonoBehaviour
{
    #region 需要的一些组件 

    /// <summary>
    /// NavMesh 组件
    /// </summary>
    public NavMeshAgent agent;

    /// <summary>
    /// 动作机
    /// </summary>
    public  Animator animator;

    /// <summary>
    /// 获取角色信息
    /// </summary>
    public Charater player;

    /// <summary>
    /// 角色当前的战斗目标
    /// </summary>
    public GameObject currenBattleTraget;

    /// <summary>
    /// 玩家复活点
    /// </summary>
    public GameObject playerRevivePoint;

    /// <summary>
    /// 复活特效
    /// </summary>
    public GameObject particleRevive;

    /// <summary>
    /// 跟随的目标
    /// </summary>
    public GameObject fllowTarget;

    /// <summary>
    /// 攻击特效
    /// </summary>
    public GameObject AttackEffect;

    /// <summary>
    /// 升级特效
    /// </summary>
    public GameObject upGradeEffect;

    //修改被攻击后的材质变化
    public Renderer renderer;
    public Material oldMaterial;
    public Material redMaterial;
    //

    public AudioClip AttackClip; //攻击音频
    public AudioClip deadClip; //死亡音频
    public GameObject damageText; //攻击伤害数字

    /// <summary>
    /// 当前玩家身上的状态
    /// </summary>
    public List<SkillState> playerSkillState;

    #endregion


    #region  父类给子类定义的方法 和一些父类已经写好的方法:Awake,Start,update, 添加动作，添加信息，寻找跟随目标，移动，
    //寻找攻击目标，玩家发动攻击，被攻击，闪避，获得经验值，升级，刷新属性恢复生命值死亡，复活， 跟随当前主控角色，玩家丢失战斗目标
    //玩家找到了攻击目标，进入战斗。  暴击, 被暴击攻击，更换材质，购买装备时计算属性
    protected virtual void Awake()
    {

    }

// Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Recover();
        SettlementStatus();
    }

    public virtual void AnimatorControl()
    {
        //添加动作
    }
    public virtual void AddInformation()
    {
        //添加信息
    }

    /// <summary>
    /// 判断当前自己是不是跟随者
    /// </summary>
    /// <returns></returns>
    public virtual bool IsFollower()
    {
        fllowTarget = FindObjectOfType<PlayerControl>().gameObject;
        if (fllowTarget != null)
        {
            if (fllowTarget.transform.name != transform.name)  //如果角色是跟随者，就返回true
            {
                return true;
            }
        }
        return false;  //如果角色是主机就返回false
    }

    /// <summary>
    /// 鼠标控制移动，不同角色攻击距离不同
    /// </summary>
    /// <param name="attackdistance"></param>
    /// <returns></returns>
    public virtual void Moving(Vector3 pos)
    {
       
    }

    /// <summary>
    /// 进入战斗距离
    /// </summary>
    public virtual void CombatDistance()
    {
        if (player.isAlive)
        {
            if (currenBattleTraget != null)
            {
                agent.stoppingDistance = player.attackDistance;
                agent.destination = currenBattleTraget.transform.position;
                transform.LookAt(currenBattleTraget.transform);
            }
        }
    }

    /// <summary>
    /// 判断是否已经接近攻击目标
    /// </summary>
    /// <returns></returns>
    public virtual bool CloseToTarget()
    {
        return false;
    }

    public virtual void PlayerAttack(WolfCha monster)
    {
        // 攻击怪物角色, 给子类用
    }

    public virtual void PlayerOnHit()
    {

    }

    /// <summary>
    /// 闪避方法 根据角色的闪避值计算
    /// </summary>
    /// <param name="cha"></param>
    /// <returns></returns>
    public virtual bool Evade(Charater cha)
    {
        int n = Random.Range(0, 100);
        if (cha.evade < n)
        {
            return true;   //闪避失败  就返回true
        }

        return false;
    }

    /// <summary>
    /// 玩家角色统一的获取经验值的方法
    /// </summary>
    public void GetExp(Charater wolf)
    {

        player.exp += wolf.level * 5 + 17;
        player.money += wolf.money;

        if (player.exp >= player.maxExp)
        {
            player.exp = player.exp - player.maxExp;
            UpGrade();
        }
    }

    public virtual void UpGrade()
    {
        //子类重写
    }

    /// <summary>
    /// 根据力量敏捷值刷新属性
    /// </summary>
    public void RefreshAttribute()
    {
        player.hp = player.basicsHp + player.power * 19;
        player.maxHp = player.hp;
        player.mp = player.basicsMp + player.mentality * 13;
        player.maxMp = player.mp;
        player.attack = player.basicsAttack + player.power;
        player.def = player.basicsDef + player.power;
        player.attackSpeed = player.basicsAttackSpeed - (float)player.agility / 200;
        player.evade = player.basicsEvade + player.agility / 10;
        player.critical = player.basicsCritical + player.agility / 8;
    }

    /// <summary>
    /// 添加buff时 计算属性
    /// </summary>
    public void CountAttribute()
    {
        player.maxHp = player.basicsHp + player.power * 19;
        player.maxMp = player.basicsMp + player.mentality * 13;
        player.attack = player.basicsAttack + player.power;
        player.def = player.basicsDef + player.power;
        player.attackSpeed = player.basicsAttackSpeed - (float)player.agility / 200;
        player.evade = player.basicsEvade + player.agility / 10;
        player.critical = player.basicsCritical + player.agility / 8;
    }

    /// <summary>
    /// 每2秒恢复一定的生命值和魔法值,并且当玩家角色的血量和魔法值超过上限的时候控制住
    /// </summary>
    float temp_Time = 0; //这个方法用到的计时器
    public virtual void Recover()
    {
        if (player.hp >= player.maxHp)
        {
            player.hp = player.maxHp;
        }
        if (player.mp >= player.maxMp)
        {
            player.mp = player.maxMp;
        }
        temp_Time += Time.deltaTime;
        if (temp_Time > 1)
        {
            player.hp += player.power / 10;
            player.mp += player.mentality / 10;
            temp_Time = 0;
        }
    }

    /// <summary>
    /// 玩家死亡
    /// </summary>
    public virtual void dead()
    {
        if (player.isAlive == false)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(deadClip, transform.position);
        currenBattleTraget = null;
        player.isAlive = false;
        agent.enabled = false;
    }

    /// <summary>
    /// 死亡后复活
    /// </summary>
    public virtual void Revive()
    {
        transform.position = playerRevivePoint.transform.position;
        agent.enabled = true;
        agent.ResetPath();
        player.hp = player.maxHp / 2;
        player.mp = player.maxMp / 2;
        player.isAlive = true;
        GameObject obj = Instantiate(particleRevive, transform.position, Quaternion.identity);
        Destroy(obj, 1f);

    }

    /// <summary>
    /// 跟随当前主机
    /// </summary>
    public void MoveToHost()
    {
        agent.stoppingDistance = 2.5f;
        agent.destination = fllowTarget.transform.position;
    }

    /// <summary>
    /// 玩家丢失当前目标
    /// </summary>
    /// <returns></returns>
    public bool PlayerLoseTarget()
    {
        if ((currenBattleTraget == null) ||
            (currenBattleTraget.GetComponent<WolfCha>().wolf.isAlive == false) ||
            (fllowTarget.GetComponent<PlayerCharater>().currenBattleTraget == null))
        //当玩家主动走开，或者当前战斗目标死亡了,或者自己跟随的对象 没有战斗了
        {
            currenBattleTraget = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 找到了攻击的目标
    /// </summary>
    /// <returns></returns>
    public bool FindTarget()
    {
        if (fllowTarget.GetComponent<PlayerCharater>().currenBattleTraget != null
    ||      currenBattleTraget != null) //如果当前主机的目标不为空   如果自己的攻击目标
        {
            currenBattleTraget = fllowTarget.GetComponent<PlayerCharater>().currenBattleTraget; //那么就把自己的目标变成主机的目标开始攻击主机的目标
            return true;
        }

        return false;
    }

    /// <summary>
    /// 暴击计算
    /// </summary>
    public bool PlayerCriticalStrike(Charater cha)
    {
        int n = Random.Range(0, 100);
        if (cha.critical > n)
        {
            return true;   //闪避失败  就返回true
        }
        return false;
    }

    /// <summary>
    /// 被暴击攻击
    /// </summary>
    public virtual void PlayerOnCriticalStrike()
    {
        
    }

    /// <summary>
    /// 更换材质
    /// </summary>
    public void ChangeMaterial()
    {
        renderer.material = oldMaterial;
    }

    /// <summary>
    /// 购买装备时，计算属性
    /// </summary>
    public void PlayerGetItemAttribute(Item item)
    {
        if (item.itemType==Item.ItemType.EQ)   //购买了装备，就加上各种属性      
        {
            player.power += item.power;
            player.agility += item.agility;
            player.mentality += item.mentality;
            player.basicsAttack += item.attack;
            player.basicsDef += item.def;
            player.basicsAttackSpeed -= item.attackSpeed/200;
            player.basicsEvade += item.evade;
            player.basicsCritical += item.critical;
            player.basicsHp += item.hp;
            player.basicsMp += item.mp;
            player.maxHp += item.hp;
            player.maxMp += item.mp;
            CountAttribute();   //加完了之后计算一下属性值
        }
    }

    /// <summary>
    /// 玩家丢弃装备
    /// </summary>
    /// <param name="item"></param>
    public void PlayerloseItemAttribute(Item item)
    {
        if (item.itemType == Item.ItemType.EQ)   //丢弃了装备，就减掉这个装备带来的各种属性      
        {
            player.power -= item.power;
            player.agility -= item.agility;
            player.mentality -= item.mentality;
            player.basicsAttack -= item.attack;
            player.basicsDef -= item.def;
            player.basicsAttackSpeed += item.attackSpeed/200;
            player.basicsEvade -= item.evade;
            player.basicsCritical -= item.critical;
            player.basicsHp -= item.hp;
            player.basicsMp -= item.mp;
            player.maxHp -= item.hp;
            player.maxMp -= item.mp;
            CountAttribute();   //减完了之后计算一下属性值
        }
    }

    /// <summary>
    /// 结算状态
    /// </summary>
    public void SettlementStatus()
    {
        if (playerSkillState.Count==0)
        {
            return;
        }
        for (int i = 0; i < playerSkillState.Count; i++)
        {
            playerSkillState[i].cd_time += Time.deltaTime;
            if (playerSkillState[i].cd_time> playerSkillState[i].time)
            {
                //删除状态
                SkillManager.Instance().RemoveState(playerSkillState[i], player);
                playerSkillState[i].haveState = false;
                playerSkillState.RemoveAt(i);
                CountAttribute();
            }
        }
    }


    #endregion
}
