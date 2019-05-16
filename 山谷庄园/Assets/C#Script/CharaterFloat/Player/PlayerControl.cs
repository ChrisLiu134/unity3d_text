using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家控制器类
/// </summary>
public class PlayerControl : MonoBehaviour {

    /// <summary>
    /// 绿色特效组件
    /// </summary>
    public GameObject efx_Click_Green;

    /// <summary>
    /// 橙色特效组件
    /// </summary>
    public GameObject efx_Click_Orange;

    /// <summary>
    /// 红色特效组件
    /// </summary>
    public GameObject efx_Click_Red;

    /// <summary>
    /// 当前玩家控制的角色的背包
    /// </summary>
    public List<Item> currenPlayerBackPack;

    /// <summary>
    /// 当前角色的技能信息
    /// </summary>
    public List<Skill> currenPlayerSkills;


    /// <summary>
    /// 判定目前是不是已经使用了技能并且在寻找目标
    /// </summary>
    public bool isUseSkillFindTarget;

    /// <summary>
    /// 当前使用的技能
    /// </summary>
    public Skill currenUseSkill;

    /// <summary>
    /// 反馈一些操作信息给玩家的组件
    /// </summary>
    public GameObject infoText; 

    public PlayerCharater playerCha;   
    FollowCamera followCamera;
    ChangeFaceImage changeFace;
    ChangeMouse changeMouse;
    PlayerControl playerControl;
    GameMode gameMode;
    UIManager uIManager;
    ShadeRay shadeRay;

    private void Awake()
    {
       
    }

    private void OnEnable()
    {
        changeMouse = FindObjectOfType<ChangeMouse>();
        shadeRay = FindObjectOfType<ShadeRay>();
        uIManager = FindObjectOfType<UIManager>();
        gameMode = FindObjectOfType<GameMode>();
        followCamera = FindObjectOfType<FollowCamera>();
        changeFace = FindObjectOfType<ChangeFaceImage>();
        playerControl = GetComponent<PlayerControl>();
        playerCha = GetComponent<PlayerCharater>();
        //playerCha.fllowTarget = gameObject;

        efx_Click_Green = GameObject.Find("Efx_Click_Green");
        efx_Click_Orange = GameObject.Find("Efx_Click_Orange");
        efx_Click_Red = GameObject.Find("Efx_Click_Red");
        infoText = GameObject.Find("ContorolInfo");
        ConfirmCurrentBackpack();
        ConfirmCurrentSkill();
    }

    // Use this for initialization
    void Start (  ) {

    }
	
	// Update is called once per frame
	void Update () {
        if (gameMode.gameState!=GameMode.EGameState.playing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            InputF1();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            InputF2();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InputEsc();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            InputP();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InputS();
        }

        if (playerCha.player.isAlive == false)
        {
            Debug.Log("玩家当前为为死亡状态");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            InputMouseZero(MousePointer());
        }

        if (Input.GetMouseButtonDown(1))
        {
            InputMouseOne(MousePointer());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            InputQ();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            InputW();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InputE();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            InputR();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            InputD();
        }
        //鼠标按照当前指向的目标去改变鼠标样式
        changeMouse.Change(MousePointer(),isUseSkillFindTarget);
    }


    #region 各种按键对应的方法
    /// <summary>
    /// 返回当前摄像头到鼠标位置的射线打击到的hit
    /// </summary>
    /// <returns></returns>
    public RaycastHit MousePointer()
    {
        int layerMask = LayerMask.GetMask("MinimapSign");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, layerMask);
        return hit;
    }

    /// <summary>
    /// 按鼠标左键  移动方法
    /// </summary>
    public void InputMouseZero(RaycastHit hit)
    {
        //如果点击鼠标左键的时候，信息面板当前显示的不是自己，那么切换回来显示自己
        if (!hit.transform.CompareTag("Player"))
        {
            if (uIManager.currenObj != gameObject)
            {
                uIManager.currenObj = gameObject;
                changeFace.ChangeFace(transform);
            }
        }
        //

        //如果当前处于释放技能的状态
        if (isUseSkillFindTarget)
        {
            if (IsSkillready(hit)) //如果技能准备就绪
            {
                SkillTakeEffect(hit);  //技能生效，
            }
            return;
        }
        //

        Vector3 pos = new Vector3(hit.point.x * 1, hit.point.y * 1, hit.point.z * 1);

        if (shadeRay.CheckUiRayCastObjects()) //如果点击的是ui上的东西，就停止移动
        {
            return;
        }

        if (hit.transform.CompareTag("Monster"))  //如果点击的是怪物
        {
            GameObject objR = Instantiate(efx_Click_Red, pos, Quaternion.identity,hit.transform);
            Destroy(objR, 0.32f);

            playerCha.agent.stoppingDistance = playerCha.player.attackDistance;
            playerCha.currenBattleTraget = hit.transform.gameObject;
        }
        else if (hit.transform.CompareTag("Npc"))  //如果点击的是npc
        {
            GameObject objO = Instantiate(efx_Click_Orange, pos, Quaternion.identity);
            Destroy(objO, 0.32f);
            playerCha.agent.stoppingDistance = 1f;
            playerCha.currenBattleTraget = null;
        }
        else 
        {
            GameObject objG = Instantiate(efx_Click_Green, pos, Quaternion.identity);
            Destroy(objG, 0.32f);
            playerCha.agent.stoppingDistance = 0f;
            playerCha.currenBattleTraget = null;
        }

        playerCha.Moving(pos);
       
    }

    /// <summary>
    /// 按鼠标右键
    /// </summary>
    public void InputMouseOne(RaycastHit hit)
    {
        //清空技能施法状态和当前使用技能
        EmptyingCurrentSkillsCastingState();

        if (hit.transform.name== "Swordman")
        {
            InputF1();
            return;
        }
        if (hit.transform.name == "Magician")
        {
            InputF2();
            return;
        }
        changeFace.InputMouseOne(MousePointer());
        PickUpItem(MousePointer().transform);
    }


    /// <summary>
    /// 按f1切换主控玩家为剑客
    /// </summary>
    void InputF1()
    {
        GameObject obj = GameObject.Find("Swordman");
        if (obj.GetComponent<PlayerControl>()==null)
        {
            obj.AddComponent<PlayerControl>();
            playerCha.fllowTarget = obj;
            Destroy(gameObject.GetComponent<PlayerControl>());
        }

        followCamera.ChangePlayer();
        changeFace.ChangeFace(obj.transform);
        uIManager.currenObj = obj;
    }

    /// <summary>
    /// 按f2切换主控角色为法师
    /// </summary>
    void InputF2()
    {
        GameObject obj = GameObject.Find("Magician");
        if (obj.GetComponent<PlayerControl>() == null)
        {
            obj.AddComponent<PlayerControl>();
            playerCha.fllowTarget = obj;
            Destroy(gameObject.GetComponent<PlayerControl>());
        }

        followCamera.ChangePlayer();
        changeFace.ChangeFace(obj.transform);
        uIManager.currenObj = obj;
    }

    /// <summary>
    /// 按esc打开关闭设置栏
    /// </summary>
    void InputEsc()
    {
        uIManager.ClickSettingGameButton();
    }

    /// <summary>
    /// 按p打开关闭商店
    /// </summary>
    void InputP()
    {
        uIManager.ClickShopButton();
    }

    /// <summary>
    /// 按s建，自动拾取
    /// </summary>
    void InputS()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.CompareTag("DropItem"))
            {
                PickUpItem(colliders[i].transform);
            }
        }
    }

    /// <summary>
    /// 当前控制的玩家捡起右键点击的道具
    /// </summary>
    void PickUpItem(Transform trans)
    {
        if (trans.CompareTag("DropItem"))
        {
            if ((transform.position- trans.position).magnitude<2)  //距离够仅的时候
            {
                if (!ISCanSynthesized(trans.GetComponent<DropItem>().dropItem))   //如果该道具不可以合成
                {
                    if (trans.GetComponent<DropItem>().dropItem.itemType == Item.ItemType.ME)
                    {
                        for (int i = 0; i < currenPlayerBackPack.Count; i++)    //计算是不是可以叠加
                        {
                            if (trans.GetComponent<DropItem>().dropItem.itemType == Item.ItemType.ME)
                            {
                                if (currenPlayerBackPack[i].id == trans.GetComponent<DropItem>().dropItem.id && currenPlayerBackPack[i].id != 0)
                                {
                                    if (currenPlayerBackPack[i].number == 99)
                                    {
                                        Debug.Log("到99上限了");
                                        break;
                                    }
                                    else
                                    {
                                        currenPlayerBackPack[i].number += trans.GetComponent<DropItem>().dropItem.number;
                                        trans.GetComponent<DropItem>().CloseItemInfoView();
                                        Destroy(trans.gameObject);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    if (GetItem(trans.GetComponent<DropItem>().dropItem))
                    {
                        Destroy(trans.gameObject);
                    }
                }
                else
                {
                    Destroy(trans.gameObject);
                }


            }
        }
    }

    #endregion

    #region 特殊方法， 判断可不可以合成， 获得道具， 丢弃道具 ， 确认当前背包
    /// <summary>
    /// 判断可不可以合成
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool ISCanSynthesized(Item item)
    {
        ItemSynthesis itemSyn = PlayerBackPackManager.Instance().CanSynthese(ItemManager.Instance().itemSyntheses, item, currenPlayerBackPack);
        if (itemSyn != null)  //如果当前装备和玩家背包中的装备组成了一个合成公式。 
        {
            PlayerBackPackManager.Instance().Synthesis(playerControl, itemSyn);  // 那么就进行合成
            return true;
        }
        return false;
    }

    /// <summary>
    /// 当前控制的玩家的到物品
    /// </summary>
    /// <param name="item"></param>
    public bool GetItem(Item item )
    {
        ConfirmCurrentBackpack();
        //每次获取装备的时候都要计算当前装备是不是可以合成
        for (int i = 0; i < currenPlayerBackPack.Count; i++)
        {
            if (currenPlayerBackPack[i].id == 0)
            {
                currenPlayerBackPack[i] = item;
                playerCha.PlayerGetItemAttribute(item);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 当前控制的玩家失去物品
    /// </summary>
    public void LoseItem(Item item)
    {
        ConfirmCurrentBackpack();

        for (int i = 0; i < currenPlayerBackPack.Count; i++)
        {
            if (currenPlayerBackPack[i]==item)
            {
                currenPlayerBackPack[i] = new Item();
                break;
            }
        }
        playerCha.PlayerloseItemAttribute(item);
    }

    /// <summary>
    /// 确认当前控制者的背包
    /// </summary>
    public void ConfirmCurrentBackpack()
    {
        if (transform.name == "Swordman")
        {
            currenPlayerBackPack = ItemManager.Instance().swordmanBackPack;
        }
        else if (transform.name == "Magician")
        {
            currenPlayerBackPack = ItemManager.Instance().magicianBackPack;
        }
    }

    /// <summary>
    /// 确认当前控制者的技能
    /// </summary>
    public void ConfirmCurrentSkill()
    {
        if (transform.name == "Swordman")
        {
            currenPlayerSkills = SkillManager.Instance().swordmanSkill;
        }
        else if (transform.name == "Magician")
        {
            currenPlayerSkills = SkillManager.Instance().magicianSkill;
        }
    }
    #endregion

    #region  技能相关

    /// <summary>
    /// 判断技能可不可以发动了
    /// </summary>
    public bool IsSkillready(RaycastHit hit)
    {
        Vector3 pos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        switch (currenUseSkill.skillType)
        {
            case Skill.SkillType.MonomerDamage:            
                //单体伤害技能只能打敌人
                if (hit.transform.CompareTag("Monster"))
                {
                    if ((hit.transform.position - transform.position).magnitude < currenUseSkill.skillDistance && hit.transform.GetComponent<WolfCha>().wolf.hp>0)  //鼠标指向的目标的距离小于施法距离,并且目标还活着。
                    {
                        return true;
                    }
                    else
                    {
                        GameObject obj = Instantiate(infoText, transform.position + new Vector3(0, 2, 1), Quaternion.identity);
                        obj.AddComponent<TextDamage>();
                        obj.GetComponent<TextDamage>().GetInfo("技能不在施法范围内", Color.yellow);
                    }
                }
                break;
            case Skill.SkillType.MonomerTreatment: 
                //单体治疗技能
                if (hit.transform.CompareTag("Monster") || hit.transform.CompareTag("Player"))
                {
                    if ((hit.transform.position - transform.position).magnitude < currenUseSkill.skillDistance )  //鼠标指向的目标的距离小于施法距离
                    {
                        return true;
                    }
                    else
                    {
                        GameObject obj = Instantiate(infoText, transform.position + new Vector3(0, 2, 1), Quaternion.identity);
                        obj.AddComponent<TextDamage>();
                        obj.GetComponent<TextDamage>().GetInfo("技能不在施法范围内", Color.yellow);
                    }
                }
                break;
            case Skill.SkillType.MonomerBuff:
                //单体buff技能
                if (hit.transform.CompareTag("Monster") || hit.transform.CompareTag("Player"))
                {
                    if ((hit.transform.position - transform.position).magnitude < currenUseSkill.skillDistance)  //鼠标指向的目标的距离小于施法距离
                    {
                        return true;
                    }
                    else
                    {
                        GameObject obj = Instantiate(infoText, transform.position + new Vector3(0, 2, 1), Quaternion.identity);
                        obj.AddComponent<TextDamage>();
                        obj.GetComponent<TextDamage>().GetInfo("技能不在施法范围内", Color.yellow);
                    }
                }
                break;
            case Skill.SkillType.GroupDamage: 
                //群体伤害性攻击技能
                if ((pos - transform.position).magnitude < currenUseSkill.skillDistance)  //鼠标指向的点的距离小于施法距离
                {
                    return true;
                }
                else
                {
                    GameObject obj = Instantiate(infoText, transform.position + new Vector3(0, 2, 1), Quaternion.identity);
                    obj.AddComponent<TextDamage>();
                    obj.GetComponent<TextDamage>().GetInfo("技能不在施法范围内", Color.yellow);
                }
                break;
            case Skill.SkillType.GroupTherapy:  //暂时没有添加这个类型的技能。
                break;
            case Skill.SkillType.GroupBuff:
                if ((pos - transform.position).magnitude < currenUseSkill.skillDistance)  //鼠标指向的点的距离小于施法距离
                {
                    return true;
                }
                else
                {
                    GameObject obj = Instantiate(infoText, transform.position + new Vector3(0, 2, 1), Quaternion.identity);
                    obj.AddComponent<TextDamage>();
                    obj.GetComponent<TextDamage>().GetInfo("技能不在施法范围内", Color.yellow);
                }
                break;
            case Skill.SkillType.AddAttribute:   //这个技能是被动不需要生效
                break;
            default:
                break;
        }

        return false;

    }

    /// <summary>
    /// 技能生效
    /// </summary>
    public void SkillTakeEffect(RaycastHit hit)
    {
        Vector3 releasePos;
        if (currenUseSkill.skillDistance == 100)
        {
            //单独设计，施法距离为100的技能， 是以自己为中心的
            releasePos = transform.position;
        }
        else
        {
            if (hit.transform!=null)
            {
                releasePos = hit.transform.position;
            }
            releasePos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        //技能特效
        GameObject obj = Instantiate(currenUseSkill.skillEffect, releasePos, Quaternion.identity);
        Destroy(obj, 1);
        //技能音效和玩家朝向被命中的目标
        AudioSource.PlayClipAtPoint(currenUseSkill.skillClip, transform.position);
        transform.LookAt(hit.transform);

        //技能面板展示
        GameObject o = Instantiate(playerControl.infoText, playerControl.transform.position + new Vector3(0, 2, 1), Quaternion.identity);
        o.AddComponent<TextDamage>();
        o.GetComponent<TextDamage>().GetInfo(currenUseSkill.name, Color.yellow);

        switch (currenUseSkill.skillType)
        {
            case Skill.SkillType.MonomerDamage:            
                //单体伤害技能只能打敌人
                hit.transform.GetComponent<WolfCha>().wolf.hp -= currenUseSkill.damage;
                if (hit.transform.GetComponent<WolfCha>().currenAttacktarget==null)
                {
                    hit.transform.GetComponent<WolfCha>().currenAttacktarget = gameObject;
                }

                break;
            case Skill.SkillType.MonomerTreatment:  
                //单体治疗技能
                if (hit.transform.GetComponent<PlayerCharater>()!=null)
                {
                    hit.transform.GetComponent<PlayerCharater>().player.hp += currenUseSkill.healHP;
                    hit.transform.GetComponent<PlayerCharater>().player.mp += currenUseSkill.healMP;
                }
                else if (hit.transform.GetComponent<WolfCha>()!=null)  
                {
                    //暂时没有给怪物加血的技能。
                }

                break;
            case Skill.SkillType.MonomerBuff:
                //单体buff性技能
                if (hit.transform.GetComponent<WolfCha>()!=null)
                {
                    if (hit.transform.GetComponent<WolfCha>().currenAttacktarget == null)
                    {
                        hit.transform.GetComponent<WolfCha>().currenAttacktarget = gameObject;
                    }
                }
               
                SkillState tempState  =  SkillManager.Instance().AddSkillState(currenUseSkill);  //创造一个新的buff 

                if (hit.transform.CompareTag("Player"))
                {
                    hit.transform.GetComponent<PlayerCharater>().playerSkillState.Add(tempState);
                    SkillManager.Instance().StatusAccounting(tempState, hit.transform.GetComponent<PlayerCharater>().player);
                    hit.transform.GetComponent<PlayerCharater>().CountAttribute();
                    tempState.haveState = true;
                }
                else if (hit.transform.CompareTag("Monster"))
                {
                    hit.transform.GetComponent<WolfCha>().monsterSkillState.Add(tempState);
                    SkillManager.Instance().StatusAccounting(tempState, hit.transform.GetComponent<WolfCha>().wolf);
                    hit.transform.GetComponent<WolfCha>().CountAttribute();
                    tempState.haveState = true;
                }

                break;
            case Skill.SkillType.GroupDamage:
                //群体攻击技能
                Collider[] colliders = Physics.OverlapSphere(releasePos, currenUseSkill.range);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].transform.CompareTag("Monster"))
                    {
                        colliders[i].transform.GetComponent<WolfCha>().wolf.hp -= currenUseSkill.damage;
                        if (colliders[i].transform.GetComponent<WolfCha>().currenAttacktarget == null)  //如果被技能打到的怪物没有攻击目标，那么就会攻击施法者
                        {
                            colliders[i].transform.GetComponent<WolfCha>().currenAttacktarget = gameObject;
                        }
                    }
                }

                break;
            case Skill.SkillType.GroupTherapy:  
                //暂时没有添加这个类型的技能。
                break;
            case Skill.SkillType.GroupBuff:
                //群体buff技能
                Collider[] colliders1 = Physics.OverlapSphere(releasePos, currenUseSkill.range);
                SkillState tempState1 = SkillManager.Instance().AddSkillState(currenUseSkill);  //创造一个新的buff 
                for (int i = 0; i < colliders1.Length; i++)
                {
                    if (colliders1[i].transform.CompareTag("Player"))
                    {
                        colliders1[i].transform.GetComponent<PlayerCharater>().playerSkillState.Add(tempState1);
                        SkillManager.Instance().StatusAccounting(tempState1, colliders1[i].transform.GetComponent<PlayerCharater>().player);
                        colliders1[i].transform.GetComponent<PlayerCharater>().CountAttribute();
                        tempState1.haveState = true;
                    }
                    else if (hit.transform.CompareTag("Monster"))
                    {
                        colliders1[i].transform.GetComponent<WolfCha>().monsterSkillState.Add(tempState1);
                        SkillManager.Instance().StatusAccounting(tempState1, colliders1[i].transform.GetComponent<WolfCha>().wolf);
                        colliders1[i].transform.GetComponent<WolfCha>().CountAttribute();
                        tempState1.haveState = true;
                    }
                }

                break;
            case Skill.SkillType.AddAttribute:   
                //这个技能是被动不需要生效
                break;
            default:
                break;
        }

        playerCha.player.mp -= currenUseSkill.skillConsume;
        playerCha.animator.SetTrigger(currenUseSkill.animatorName);
        currenUseSkill.isCd = true;
        EmptyingCurrentSkillsCastingState();
    }


    /// <summary>
    /// 清空当前技能和施法状态
    /// </summary>
    public void EmptyingCurrentSkillsCastingState()
    {
        currenUseSkill = null;
        isUseSkillFindTarget = false;
    }

    public void InputQ()
    {
        uIManager.skillsGrids[0].GetComponent<SkillControl>().CilckPlusPoint();
    }
    public void InputW()
    {
        uIManager.skillsGrids[1].GetComponent<SkillControl>().CilckPlusPoint();
    }
    public void InputE()
    {
        uIManager.skillsGrids[2].GetComponent<SkillControl>().CilckPlusPoint();
    }
    public void InputR()
    {
        uIManager.skillsGrids[3].GetComponent<SkillControl>().CilckPlusPoint();
    }
    public void InputD()
    {
        uIManager.skillsGrids[4].GetComponent<SkillControl>().CilckPlusPoint();
    }

    #endregion
}
