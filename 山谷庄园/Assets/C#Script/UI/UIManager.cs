using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using LitJson;
using System.IO;     //读取文件时要引用这个
using System.Text;

public class UIManager : MonoBehaviour {

    #region public 出来的组件 各种gameobject text slider button
    //力量 敏捷 智力 text
    public Text power;
    public Text agility;
    public Text mentality;
    //

    // 角色信息， 名字，等级，攻击，防御 等等
    public Text attack;
    public Text def;
    public Text attackSpeed;
    public Text evade;
    public Text critical;
    public Text targetName;
    public Text myName;
    public Text id;
    //

    //经验条 等级信息；
    public Image expSlider;
    public Text level;
    //

    //血条 蓝条， 数字信息
    public Image hpSlider;
    public Text hpNumber;
    public Image mpSlider;
    public Text mpNumber;
    //

    //设置按键
    public Button settingButton;
    public GameObject settingWindow;
    public Button saveButton;
    public Button quitGameButton;
    //

    //商店按键
    public Button shopButton;
    public GameObject shopWindow;
    //

    //开始界面
    public GameObject startWindow;
    public GameObject pressAnyKey;
    public GameObject startAndeLoad;
    //

    //金钱展示
    public Text moneyText;
    //

    //技能点信息
    public Text skillPointText;

    //显示保存成功
    public GameObject saveText;

    #endregion

    //调用其它类的方法需要的组件
    GameMode gameMode;
    StoreManager storeManager;

    /// <summary>
    /// 当前需要展示信息的目标, 左键点击角色后会改变这个值
    /// </summary>
    public GameObject currenObj;

    /// <summary>
    /// 外挂按钮，用来演示的
    /// </summary>
    public Button plug_Button;

    /// <summary>
    /// 点击音乐
    /// </summary>
    public AudioClip clickClip;


    /// <summary>
    /// 背包的格子
    /// </summary>
    [HideInInspector]
    public GameObject[] BackPackGrids;

    /// <summary>
    /// 技能栏的格子
    /// </summary>
    [HideInInspector]
    public GameObject[] skillsGrids;

    /// <summary>
    /// 状态栏的格子
    /// </summary>
    [HideInInspector]
    public GameObject[] skillStateGrids;




    PlayerAttack player;
    public void Awake()
    {
        ItemManager.Instance().LoadItemJson();
        ItemManager.Instance().LoadSynthesisJson();
        ItemManager.Instance().SetBackPackItem();

        SkillManager.Instance().LoadSkillJson();
        SkillManager.Instance().PlayerChaGetSkill();
    }
    // Use this for initialization
    void Start () {
        gameMode = FindObjectOfType<GameMode>();
        storeManager = FindObjectOfType<StoreManager>();
        //找到当前背包的六个格子
        BackPackGrids = GameObject.FindGameObjectsWithTag("BackPackGrid").OrderBy(g => g.transform.GetSiblingIndex()).ToArray();  //让组件按照在场景中hierarchy里从上到下的顺序排好序
        skillsGrids = GameObject.FindGameObjectsWithTag("SkillGrid").OrderBy(g => g.transform.GetSiblingIndex()).ToArray();
        skillStateGrids= GameObject.FindGameObjectsWithTag("SkillStateGrid").OrderBy(g => g.transform.GetSiblingIndex()).ToArray();

        for (int i = 0; i < skillStateGrids.Length; i++)
        {
            skillStateGrids[i].SetActive(false);  //先关闭显示技能栏
        }
    }
	
	// Update is called once per frame
	void Update () {
        //显示信息面板 背包界面 技能面板
        if (currenObj != null)
        {
            ShowCurrenChaInfo();
       
            if (currenObj.CompareTag("Player")&& currenObj.GetComponent<PlayerControl>()!=null)  
            {
                //刷新背包格子数据
                RefreshBackPackBar(currenObj.GetComponent<PlayerControl>().currenPlayerBackPack);
                RefreshSkillBar(currenObj.GetComponent<PlayerControl>().currenPlayerSkills);
                RefreshSkillStateBar(currenObj.GetComponent<PlayerControl>().playerCha.playerSkillState);
            }
            else if(currenObj.CompareTag("Monster"))
            {
                RefreshBackPackBar(currenObj.GetComponent<WolfCha>().wolfBackPack);
                RefreshSkillStateBar(currenObj.GetComponent<WolfCha>().monsterSkillState);
            }

        }
       
    }

    /// <summary>
    /// 打开某窗口
    /// </summary>
    /// <param name="obj"></param>
    public void OpenWindow(GameObject obj)
    {
        obj.SetActive(true);
    }

    /// <summary>
    /// 关掉某窗口
    /// </summary>
    /// <param name="obj"></param>
    public void CloseWindow(GameObject obj)
    {
        obj.SetActive(false);
    }




    #region 按键功能的方法

    /// <summary>
    /// 点击开始游戏按键
    /// </summary>
    public void ClickStartGameButton()
    {
        gameMode.ChangeState(GameMode.EGameState.playing);
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
    }


    /// <summary>
    /// 点击游戏设置按键
    /// </summary>
    public void ClickSettingGameButton()
    {
        if (!settingWindow.activeSelf)
        {
            OpenWindow(settingWindow);
        }
        else
        {
            CloseWindow(settingWindow);
        }
    }

    /// <summary>
    /// 点击商店按钮
    /// </summary>
    public void ClickShopButton()
    {
        if (!shopWindow.activeSelf)
        {
            OpenWindow(shopWindow);
            storeManager.OpenShopWindow();
            storeManager.ShowStoreItem();
        }
        else
        {
            CloseWindow(shopWindow);
            storeManager.ResetStoreGrid();
        }
    }

    #region 关于保存游戏和读取

    /// <summary>
    /// 保存游戏玩家的角色信息为json文件
    /// </summary>
    JsonData jsonData_Player = new JsonData();


    /// <summary>
    /// 点击读取游戏按键
    /// </summary>
    public void ClickLoadGameButton()
    {
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
        LoadChaInfo();
        LoadItemInfo();
        LoadSkillInfo();
        gameMode.ChangeState(GameMode.EGameState.playing);
    }

    /// <summary>
    /// 读取角色信息
    /// </summary>
    public void LoadChaInfo()
    {
        byte[] byData = new byte[10240];
        int len = 0;
        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + "\\save\\PlayerCha.json", FileMode.OpenOrCreate);
            file.Seek(0, SeekOrigin.Begin);
            len = file.Read(byData, 0, 10240);
            //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,
            //它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.
            file.Close();
        }
        catch (IOException e)
        {
            Debug.Log("异常");
            Debug.Log(e.Message);
        }
        string s = Encoding.Default.GetString(byData, 0, len);  // 转码
        if (s.Length == 0)
        {
            Debug.Log("文件为空");
            return;
        }
        jsonData_Player = JsonMapper.ToObject(s);
        var info = jsonData_Player["info"];

        List<Charater> chas = new List<Charater>();
        for (int i = 0; i < info.Count; i++)
        {
            string name = info[i]["name"].ToString();
            int id = (int)info[i]["id"];
            int level = (int)info[i]["level"];
            int power = (int)info[i]["power"];
            int agility = (int)info[i]["agility"];
            int mentality = (int)info[i]["mentality"];
            int hp = (int)info[i]["hp"];
            int basicsHp = (int)info[i]["basicsHp"];
            int maxHp = (int)info[i]["maxHp"];
            int mp = (int)info[i]["mp"];
            int basicsMp = (int)info[i]["basicsMp"];
            int maxMp = (int)info[i]["maxMp"];
            int exp = (int)info[i]["exp"];
            int maxExp = (int)info[i]["maxExp"];
            int attack = (int)info[i]["attack"];
            int basicsAttack = (int)info[i]["basicsAttack"];
            int def = (int)info[i]["def"];
            int basicsDef = (int)info[i]["basicsDef"];
            string attackSpeed = info[i]["attackSpeed"].ToString();
            string basicsAttackSpeed = info[i]["basicsAttackSpeed"].ToString();
            float attacks = 0.00f;
            float.TryParse(attackSpeed,out attacks);
            float bascisAttacks = 0.00f;
            float.TryParse(basicsAttackSpeed, out bascisAttacks);
            int evade = (int)info[i]["evade"];
            int basicsEvade = (int)info[i]["basicsEvade"];
            int critical = (int)info[i]["critical"];
            int basicsCritical = (int)info[i]["basicsCritical"];
            int money = (int)info[i]["money"];
            int skillPoints = (int)info[i]["skillPoints"];
            Charater cha = new Charater();
            cha.name = name;
            cha.id = id;
            cha.level = level;
            cha.power = power;
            cha.agility = agility;
            cha.mentality = mentality;
            cha.hp = hp;
            cha.basicsHp = basicsHp;
            cha.maxHp = maxHp;
            cha.mp = mp;
            cha.basicsMp = basicsMp;
            cha.maxMp = maxMp;
            cha.attack = attack;
            cha.basicsAttack = basicsAttack;
            cha.def = def;
            cha.basicsDef = def;
            cha.attackSpeed = attacks;
            cha.basicsAttackSpeed = bascisAttacks;
            cha.evade = evade;
            cha.basicsEvade = basicsEvade;
            cha.critical = critical;
            cha.basicsCritical = basicsCritical;
            cha.money = money;
            cha.skillPoints = skillPoints;
            cha.isAlive = true;
            chas.Add(cha);

        }

        PlayerCharater[] players = FindObjectsOfType<PlayerCharater>();
        for (int i = 0; i < chas.Count; i++)
        {
            for (int j = 0; j < players.Length; j++)
            {
                if (players[j].player.id == chas[i].id)
                {
                    chas[i].attackDistance = players[j].player.attackDistance;
                    players[j].player = chas[i];
                }
            }
        }
    }


    /// <summary>
    /// 保存游戏按钮
    /// </summary>
    public void ClickSaveButton()
    {
        SaveCha();
        SaveItem();
        SaveSkillInfo();
        saveText.SetActive(true);
        Invoke("CloseSaveText",2);
    }

    /// <summary>
    /// 关掉保存成功的显示
    /// </summary>
    public void CloseSaveText()
    {
        saveText.SetActive(false);
    }

    /// <summary>
    /// 保存的方法
    /// </summary>
    public void SaveCha()
    {
        jsonData_Player["info"] = new JsonData();
        PlayerCharater[] players = FindObjectsOfType<PlayerCharater>();
        for (int i = 0; i < players.Length; i++)
        {
            SavePlayerChaInfo(players[i].player);
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath+"\\save");
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        FileStream fs = new FileStream(Application.persistentDataPath + "\\save\\PlayerCha.json", FileMode.Create);
        byte[] b = UTF8Encoding.UTF8.GetBytes(jsonData_Player.ToJson());    //以utf8的格式写入
        //开始写入
        fs.Write(b, 0, b.Length);
        //清空缓存区， 关闭流
        fs.Flush();
        fs.Close();

    }

    /// <summary>
    /// 保存玩家的信息到json
    /// </summary>
    public void SavePlayerChaInfo(Charater cha)
    {
        JsonData json = new JsonData();
        json["name"]=cha.name;
        json["id"] = cha.id;
        json["level"] = cha.level;
        json["power"] = cha.power;
        json["agility"] = cha.agility;
        json["mentality"] = cha.mentality;
        json["hp"] = cha.hp;
        json["basicsHp"] = cha.basicsHp;
        json["maxHp"] = cha.maxHp;
        json["mp"] = cha.mp;
        json["basicsMp"] = cha.basicsMp;
        json["maxMp"] = cha.maxMp;
        json["exp"] = cha.exp;
        json["maxExp"] = cha.maxExp;
        json["attack"] = cha.attack;
        json["basicsAttack"] = cha.basicsAttack;
        json["def"] = cha.def;
        json["basicsDef"] = cha.basicsDef;
        json["attackSpeed"] = cha.attackSpeed;
        json["basicsAttackSpeed"] = cha.basicsAttackSpeed;
        json["evade"] = (int)cha.evade;
        json["basicsEvade"] = (int)cha.basicsEvade;
        json["critical"] = (int)cha.critical;
        json["basicsCritical"] = (int)cha.basicsCritical;
        json["money"] = cha.money;
        json["skillPoints"] = cha.skillPoints;
        jsonData_Player["info"].Add(json);
    }


    /// <summary>
    /// 保存游戏玩家的角色信息为json文件
    /// </summary>
    JsonData jsonData_Item= new JsonData();

    /// <summary>
    /// 保存玩家身上的道具
    /// </summary>
    public void SaveItem()
    {

        jsonData_Item["info"] = new JsonData();
        for (int i = 0; i < ItemManager.Instance().swordmanBackPack.Count; i++)
        {
            JsonData json = new JsonData();
            json["itemID"] = ItemManager.Instance().swordmanBackPack[i].id;
            json["itemType"] = (int)ItemManager.Instance().swordmanBackPack[i].itemType;
            json["itemNumber"]= (int)ItemManager.Instance().swordmanBackPack[i].number;
            jsonData_Item["info"].Add(json);
        }

        for (int i = 0; i < ItemManager.Instance().magicianBackPack.Count; i++)
        {
            JsonData json = new JsonData();
            json["itemID"] = ItemManager.Instance().magicianBackPack[i].id;
            json["itemType"] = (int)ItemManager.Instance().magicianBackPack[i].itemType;
            json["itemNumber"] = (int)ItemManager.Instance().magicianBackPack[i].number;
            jsonData_Item["info"].Add(json);
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "\\save");
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        FileStream fs = new FileStream(Application.persistentDataPath + "\\save\\PlayerItem.json", FileMode.Create);
        byte[] b = UTF8Encoding.UTF8.GetBytes(jsonData_Item.ToJson());    //以utf8的格式写入
        //开始写入
        fs.Write(b, 0, b.Length);
        //清空缓存区， 关闭流
        fs.Flush();
        fs.Close();
    }

    /// <summary>
    /// 读取道具信息
    /// </summary>
    public void LoadItemInfo()
    {
        byte[] byData = new byte[10240];
        int len = 0;
        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + "\\save\\PlayerItem.json", FileMode.OpenOrCreate);
            file.Seek(0, SeekOrigin.Begin);
            len = file.Read(byData, 0, 10240);
            //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,
            //它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.
            file.Close();
        }
        catch (IOException e)
        {
            Debug.Log("异常");
            Debug.Log(e.Message);
        }
        string s = Encoding.Default.GetString(byData, 0, len);  // 转码
        if (s.Length==0)
        {
            Debug.Log("文件为空");
            return;
        }
        jsonData_Item = JsonMapper.ToObject(s);
        var info = jsonData_Item["info"];
        List<Item> tempItems = new List<Item>();

        for (int i = 0; i < info.Count; i++)
        {
            int id = (int)info[i]["itemID"];
            int itemType= (int)info[i]["itemType"];
            int itemNumber = (int)info[i]["itemNumber"];
            Item item = new Item();
            item.id = id;
            item.itemType = (Item.ItemType)itemType;
            item.number = itemNumber;
            tempItems.Add(item);
        }
        //玩家的背包读取装备 因为每个玩家的背包格子都是6个
        for (int i = 0; i < 6; i++)
        {
            if (tempItems[i].itemType == Item.ItemType.EQ)
            {
                if (tempItems[i].id!=0)
                {
                    ItemManager.Instance().swordmanBackPack[i] = ItemManager.Instance().EQItemsShop[tempItems[i].id];
                }
            }
            else if (tempItems[i].itemType == Item.ItemType.ME)
            {
                Item temp = Item.Coby(ItemManager.Instance().MEItemsShop[tempItems[i].id]);
                ItemManager.Instance().swordmanBackPack[i] = temp;
                ItemManager.Instance().swordmanBackPack[i].number = tempItems[i].number;
            }
            else if (tempItems[i].itemType == Item.ItemType.RB)
            {
                ItemManager.Instance().swordmanBackPack[i] = ItemManager.Instance().RBItemsShop[tempItems[i].id];
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (tempItems[i+6].itemType == Item.ItemType.EQ)
            {
                if (tempItems[i+6].id != 0)
                {
                    ItemManager.Instance().magicianBackPack[i] = ItemManager.Instance().EQItemsShop[tempItems[i+6].id];
                }
            }
            else if (tempItems[i+6].itemType == Item.ItemType.ME)
            {
                Item temp = Item.Coby(ItemManager.Instance().MEItemsShop[tempItems[i+6].id]);
                ItemManager.Instance().magicianBackPack[i] = temp;
                ItemManager.Instance().magicianBackPack[i].number = tempItems[i+6].number;
            }
            else if(tempItems[i+6].itemType == Item.ItemType.RB)
            {
                ItemManager.Instance().magicianBackPack[i] = ItemManager.Instance().RBItemsShop[tempItems[i+6].id];
            }
        }
    }

    /// <summary>
    /// 保存游戏玩家的角色信息为json文件
    /// </summary>
    JsonData jsonData_Skill = new JsonData();

    /// <summary>
    /// 保存技能信息
    /// </summary>
    public void SaveSkillInfo()
    {
        jsonData_Skill["info"] = new JsonData();
        for (int i = 0; i < SkillManager.Instance().swordmanSkill.Count; i++)
        {
            JsonData json = new JsonData();
            json["skillId"] = SkillManager.Instance().swordmanSkill[i].id;
            json["skillLevel"] = SkillManager.Instance().swordmanSkill[i].level;
            jsonData_Skill["info"].Add(json);
        }

        for (int i = 0; i < SkillManager.Instance().magicianSkill.Count; i++)
        {
            JsonData json = new JsonData();
            json["skillId"] = SkillManager.Instance().magicianSkill[i].id;
            json["skillLevel"] = SkillManager.Instance().magicianSkill[i].level;
            jsonData_Skill["info"].Add(json);
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath + "\\save");
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
        FileStream fs = new FileStream(Application.persistentDataPath + "\\save\\PlayerSkill.json", FileMode.Create);
        byte[] b = UTF8Encoding.UTF8.GetBytes(jsonData_Skill.ToJson());    //以utf8的格式写入
        //开始写入
        fs.Write(b, 0, b.Length);
        //清空缓存区， 关闭流
        fs.Flush();
        fs.Close();
    }

    /// <summary>
    /// 读取技能信息
    /// </summary>
    public void LoadSkillInfo()
    {
        byte[] byData = new byte[10240];
        int len = 0;
        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + "\\save\\PlayerSkill.json", FileMode.OpenOrCreate);
            file.Seek(0, SeekOrigin.Begin);
            len = file.Read(byData, 0, 10240);
            //byData传进来的字节数组,用以接受FileStream对象中的数据,第2个参数是字节数组中开始写入数据的位置,
            //它通常是0,表示从数组的开端文件中向数组写数据,最后一个参数规定从文件读多少字符.
            file.Close();
        }
        catch (IOException e)
        {
            Debug.Log("异常");
            Debug.Log(e.Message);
        }
        string s = Encoding.Default.GetString(byData, 0, len);  // 转码
        if (s.Length == 0)
        {
            Debug.Log("文件为空");
            return;
        }
        jsonData_Skill = JsonMapper.ToObject(s);
        var info = jsonData_Skill["info"];
        for (int i = 0; i < info.Count; i++)
        {
            int id = (int)info[i]["skillId"];
            int level = (int)info[i]["skillLevel"];
            foreach (var skill in SkillManager.Instance().skillDic)
            {
                if (skill.Key == id)
                {
                    skill.Value.level = level;
                }
            }
        }
    }

    #endregion


    /// <summary>
    /// 点击结束游戏按钮
    /// </summary>
    public void ClickQuitGameButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }

    /// <summary>
    /// 点击外挂按钮
    /// </summary>
    public void ClickPlugButton()
    {
        AudioSource.PlayClipAtPoint(clickClip, transform.position);
        if (currenObj!=null)
        {
            if (currenObj.GetComponent<PlayerCharater>()!=null)
            {
                if (currenObj.GetComponent<PlayerCharater>().player.level==99)
                {
                    return;
                }
                currenObj.GetComponent<PlayerCharater>().UpGrade();
                if (currenObj.GetComponent<PlayerCharater>().player.money >= 999999)
                {
                    return;
                }
                currenObj.GetComponent<PlayerCharater>().player.money += 1000;
            }
        }
    }

    #endregion

    #region  显示信息， 显示角色属性， 显示当前角色的背包方法。
    /// <summary>
    /// 显示当前指向角色的信息
    /// </summary>
    public void ShowCurrenChaInfo()
    {
        Charater currenCha = new Charater();

        if (currenObj.transform.CompareTag("Player"))
        {
            currenCha = currenObj.GetComponent<PlayerCharater>().player;
            if (currenObj.GetComponent<PlayerCharater>().currenBattleTraget!=null)
            {
                targetName.text = "目标 : " + currenObj.GetComponent<PlayerCharater>().currenBattleTraget.GetComponent<WolfCha>().wolf.name;
            }
            else
            {
                targetName.text = "目标 ：";
            }
            RefreshInfo(currenCha);
        }
        else if (currenObj.transform.CompareTag("Monster"))
        {
            currenCha = currenObj.GetComponent<WolfCha>().wolf;
            if (currenObj.GetComponent<WolfCha>().currenAttacktarget != null)
            {
                targetName.text = "目标 : " + currenObj.GetComponent<WolfCha>().currenAttacktarget.GetComponent<PlayerCharater>().player.name;
            }
            else
            {
                targetName.text = "目标 ：";
            }
            RefreshInfo(currenCha);
        }
        else
        {
            Debug.Log("这个目标不是玩家或者怪物"+currenObj.transform.name);
            RefreshInfo(new Charater());
            return;
        }
    }

    /// <summary>
    /// 刷新ui面板的信息
    /// </summary>
    public void RefreshInfo(Charater cha)
    {
        if (cha==null)
        {
            Debug.Log("CHA是null");
            return;
        }
        power.text = "力量 : " + cha.power;
        agility.text = "敏捷 : " + cha.agility;
        mentality.text = "智力 : " + cha.mentality;
        attack.text = "攻击 : " + cha.attack;
        def.text = "防御 : " + cha.def;
        attackSpeed.text = "攻速 : " + cha.attackSpeed.ToString("0.00");
        evade.text = "闪避 : " + cha.evade;
        critical.text = "暴击 : " + cha.critical;
        myName.text = "名字 : " + cha.name;
        id.text = "编号 : " + cha.id;
        moneyText.text = cha.money.ToString();
        level.text = cha.level.ToString();
        hpNumber.text = cha.hp + " / " + cha.maxHp;
        mpNumber.text = cha.mp + " / " + cha.maxMp;
        skillPointText.text = "技能点: "+cha.skillPoints.ToString();
        hpSlider.fillAmount = (float)cha.hp / (float)cha.maxHp;
        mpSlider.fillAmount = (float)cha.mp/ (float)cha.maxMp;
        expSlider.fillAmount = (float)cha.exp / (float)cha.maxExp;
    }


    /// <summary>
    /// 显示和即时刷新当前角色背包里的数据
    /// </summary>
    public void RefreshBackPackBar(List<Item> currenChaBackPack)
    {

        if (BackPackGrids.Length == 0)
        {
            Debug.Log("没有找到当前背包栏");
            return;
        }

        if (currenChaBackPack == null)
        {
            Debug.Log("当前背包里没有东西");
            return;
        }

        for (int i = 0; i < currenChaBackPack.Count && i < BackPackGrids.Length; i++)   //更新当前玩家身上的物品
        {
            if (currenChaBackPack[i].id != 0)  //当玩家背包中当前的物品id不是0时， 为0时就是还没有物品。  也就是玩家购买物品，购买物品后会修改掉之前背包里面的item的数据。  id不可能是0
            {
                BackPackGrids[i].GetComponent<PlayerBackpackItem>().GetItemInfo(currenChaBackPack[i]);
                BackPackGrids[i].GetComponent<Image>().sprite = currenChaBackPack[i].spr;
                if (currenChaBackPack[i].number == 0)
                {
                    BackPackGrids[i].GetComponentInChildren<Text>().text = "1";
                }
                else
                {
                    BackPackGrids[i].GetComponentInChildren<Text>().text = currenChaBackPack[i].number.ToString();
                }
            }
            else   //如果这个栏位是空，也就是id是0  玩家还没有购买物品。
            {
                //显示原始的格子贴图 信息板也是空。
                BackPackGrids[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI1");
                BackPackGrids[i].GetComponentInChildren<Text>().text = "";
            }
        }
    }

    /// <summary>
    /// 显示当前使用者的技能栏的技能
    /// </summary>
    public void RefreshSkillBar(List<Skill> currenChaSkill)
    {
        if (skillsGrids.Length == 0)
        {
            Debug.Log("没有找到当前技能栏");
            return;
        }

        if (currenChaSkill.Count == 0)
        {
            Debug.Log("当前技能没有读取成功");
            return;
        }

        for (int i = 0; i < skillsGrids.Length&& i<currenChaSkill.Count; i++)
        {
            skillsGrids[i].GetComponent<Image>().sprite = currenChaSkill[i].skillSpr;
            skillsGrids[i].GetComponent<SkillControl>().skill = currenChaSkill[i];
            skillsGrids[i].GetComponentInChildren<Text>().text = currenChaSkill[i].level.ToString();
        }
    }

    public void RefreshSkillStateBar(List<SkillState>currenChaSkillState)
    {
        if (skillStateGrids.Length == 0)
        {
            Debug.Log("没有找到当前技能栏");
            return;
        }

        if (currenChaSkillState.Count==0)
        {
            for (int i = 0; i < skillStateGrids.Length; i++)
            {
                skillStateGrids[i].SetActive(false);
                skillStateGrids[i].GetComponent<Image>().sprite = null;
                skillStateGrids[i].GetComponent<SkillStateGrid>().skillState = new SkillState();
            }
            return;
        }

        for (int i = 0; i < skillStateGrids.Length && i < currenChaSkillState.Count; i++)
        {
            if (skillStateGrids[i].GetComponent<SkillStateGrid>().skillState.cd_time==0)
            {
                skillStateGrids[i].SetActive(true);
                skillStateGrids[i].GetComponent<Image>().sprite = currenChaSkillState[i].stateSpr;
                skillStateGrids[i].GetComponent<SkillStateGrid>().skillState = currenChaSkillState[i];
            }

        }
    }
    #endregion
}
