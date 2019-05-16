using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;       //调用json时要引用
using System.IO;     //读取文件时要引用这个
using System.Text;

public class ItemManager  {
    private static ItemManager _instance;
    /// <summary>
    /// 背包管理器单例子
    /// </summary>
    public static ItemManager Instance()
    {
        if (_instance==null)
        {
            _instance = new ItemManager();
        }
        return _instance;
    }

    #region  读取所有装备，商品分类，设置3个商店的商品， 设置玩家初始背包

    /// <summary>
    /// 装备道具商店
    /// </summary>
    public Dictionary<int, Item> EQItemsShop = new Dictionary<int, Item>();

    /// <summary>
    /// 药水道具商店
    /// </summary>
    public Dictionary<int, Item> MEItemsShop = new Dictionary<int, Item>();

    /// <summary>
    /// 卷轴道具商店
    /// </summary>
    public Dictionary<int ,Item> RBItemsShop = new Dictionary<int , Item >();  

    /// <summary>
    /// 剑士的背包   背包格子只有6个
    /// </summary>
    public  List<Item> swordmanBackPack = new List<Item>();

    /// <summary>
    /// 法师的背包   玩家的背包格子只有6个
    /// </summary>
    public List<Item> magicianBackPack = new List<Item>();

    /// <summary>
    /// 读取json文件用
    /// </summary>
    JsonData jsonData;

    /// <summary>
    /// 解析json文件
    /// </summary>
    public void LoadItemJson()
    {
        jsonData = JsonMapper.ToObject(Resources.Load<TextAsset>("Json/item").text);
        var info = jsonData["info"];
        for (int i = 0; i < info.Count; i++)
        {
            int itemtype = (int)info[i]["itemType"];
            if (itemtype==1)
            {
                int power = (int)info[i]["power"];
                int agility = (int)info[i]["agility"];
                int mentality = (int)info[i]["mentality"];
                string name = info[i]["name"].ToString();
                int id = (int)info[i]["id"];
                string itemInfo = info[i]["itemInfo"].ToString();
                int price = (int)info[i]["price"];
                string sprPath = info[i]["sprPath"].ToString();
                int attack = (int)info[i]["attack"];
                int def = (int)info[i]["def"];
                int hp = (int)info[i]["hp"];
                int mp = (int)info[i]["mp"];
                int attackSpeed = (int)info[i]["attackSpeed"];
                int evade = (int)info[i]["evade"];
                int critical = (int)info[i]["critical"];
                Item item = Item.NewEQ(power,agility,mentality,name,id,itemInfo,price,sprPath,attack,def,hp,mp,attackSpeed,evade,critical);
                EQItemsShop.Add(item.id,item);
            }
            else if (itemtype ==2)
            {
                string name = info[i]["name"].ToString();
                int id = (int)info[i]["id"];
                string itemInfo = info[i]["itemInfo"].ToString();
                int price = (int)info[i]["price"];
                string sprPath = info[i]["sprPath"].ToString();
                int hp = (int)info[i]["hp"];
                int mp = (int)info[i]["mp"];
                int number = (int)info[i]["number"];
                Item item = Item.NewME( name, id, itemInfo, price, sprPath,  hp, mp, number);
                MEItemsShop.Add(item.id,item);
            }
            else if (itemtype==3)
            {
                string name = info[i]["name"].ToString();
                int id = (int)info[i]["id"];
                string itemInfo = info[i]["itemInfo"].ToString();
                int price = (int)info[i]["price"];
                string sprPath = info[i]["sprPath"].ToString();
                Item item = Item.NewRB( name, id, itemInfo, price, sprPath);
                RBItemsShop.Add(item.id,item);
            }
        }

        Debug.Log("装备数量" + EQItemsShop.Count + "药品数量" + MEItemsShop.Count + "卷轴数量" + RBItemsShop.Count);
    }


    /// <summary>
    /// 返回当前商店页面显示的商品数据， 靠近某一个商人就显示该商人类型的商品
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, Item> StoreType()
    {
        if (GameObject.FindObjectOfType<PlayerControl>() == null)
        {
            Debug.Log("找不到玩家");
            return null ;
        }

        GameObject obj = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        Collider[] colliders = Physics.OverlapSphere(obj.transform.position, 3f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform.name == "Weapon_Npc")
            {
                return EQItemsShop;
            }
            else if (colliders[i].transform.name == "Potion_Npc")
            {
                return MEItemsShop;
            }
            else if (colliders[i].transform.name == "Bar_Npc")
            {
                return RBItemsShop;
            }
        }
        return null;
    }

    /// <summary>
    /// 开始时设置一次背包的所有物品
    /// </summary>
    public void SetBackPackItem()
    {
        for (int i = 0; i < 6; i++)
        {
            magicianBackPack.Add(new Item());
            swordmanBackPack.Add(new Item());
        }
    }
    #endregion

    #region 读取合成公式文件

    JsonData jsonData1;

    public List<ItemSynthesis> itemSyntheses = new List<ItemSynthesis>();

    /// <summary>
    /// 读取合成公式json文件
    /// </summary>
    public void LoadSynthesisJson()
    {
        jsonData1 = JsonMapper.ToObject(Resources.Load<TextAsset>("Json/SynthesisFormula").text);
        var info = jsonData1["info"];
        for (int i = 0; i < info.Count; i++)
        {
            int id = (int)info[i]["id"];
            int id1 = (int)info[i]["id1"];
            int id2 = (int)info[i]["id2"];
            int id3 = (int)info[i]["id3"];
            ItemSynthesis itemSynthesis = new ItemSynthesis(id, id1, id2, id3);
            itemSyntheses.Add(itemSynthesis);
        }
        Debug.Log("合成公式的数量"+itemSyntheses.Count);
    }
    #endregion
}
