using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具类
/// </summary>
public class Item  {
    /// <summary>
    /// 装备的类型枚举
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// 装备
        /// </summary>
        EQ,
        /// <summary>
        /// 药品
        /// </summary>
        ME,
        /// <summary>
        /// 合成书
        /// </summary>
        RB
    }
    public ItemType itemType;

    /// <summary>
    /// 物品的力量
    /// </summary>
    public int power;
    /// <summary>
    /// 物品的敏捷
    /// </summary>
    public int agility;
    /// <summary>
    /// 物品的智力
    /// </summary>
    public int mentality;
    /// <summary>
    /// 物品的名字
    /// </summary>
    public string name;
    /// <summary>
    /// 物品的编号
    /// </summary>
    public int id;
    /// <summary>
    /// 数量
    /// </summary>
    public int number;
    /// <summary>
    /// 物品的描述信息
    /// </summary>
    public string info; 
    /// <summary>
    /// 物品价格
    /// </summary>
    public int price;
    /// <summary>
    /// 物品的贴图
    /// </summary>
    public Sprite spr;
    /// <summary>
    /// 物品的攻击力
    /// </summary>
    public int attack;
    /// <summary>
    /// 物品的防御力
    /// </summary>
    public int def;
    /// <summary>
    /// 物品的生命值
    /// </summary>
    public int hp;
    /// <summary>
    /// 物品的魔法值
    /// </summary>
    public int mp;
   /// <summary>
   /// 物品的攻击速度
   /// </summary>
    public float attackSpeed;
    /// <summary>
    /// 物品的闪避
    /// </summary>
    public float evade;
    /// <summary>
    /// 物品的暴击
    /// </summary>
    public float critical;

    /// <summary>
    /// 添加新的装备 
    /// </summary>
    public static Item NewEQ(int _power, int _agility, int _mentality, string _name, int _id, string _info, int _price, string _sprPath, int _attack, int _def, int _hp, int _mp, float _attackSpeed, float _evade,float _critical)
    {
        Item item = new Item();
        item.itemType = ItemType.EQ;
        item.power = _power;
        item.agility = _agility;
        item.mentality = _mentality;
        item.name = _name;
        item.id = _id;
        item.info = _info;
        item.price = _price;
        item.spr= Resources.Load<Sprite>(_sprPath);
        item.attack = _attack;
        item.def = _def;
        item.hp = _hp;
        item.mp = _mp;
        item.attackSpeed = _attackSpeed;
        item.evade = _evade;
        item.critical = _critical;
        return item;
    }

    /// <summary>
    /// 添加新的药品
    /// </summary>
    public static Item NewME( string _name, int _id, string _info, int _price, string _sprPath, int _hp, int _mp,int _number)
    {
        Item item = new Item();
        item.itemType = ItemType.ME;
        item.name = _name;
        item.id = _id;
        item.info = _info;
        item.price = _price;
        item.spr= Resources.Load<Sprite>(_sprPath);
        item.hp = _hp;
        item.mp = _mp;
        item.number = _number;
        return item;
    }

    /// <summary>
    /// 添加新的卷轴
    /// </summary>
    public static Item NewRB(string _name, int _id, string _info, int _price, string _sprPath)
    {
        Item item = new Item();
        item.itemType = ItemType.RB;
        item.name = _name;
        item.id = _id;
        item.info = _info;
        item.price = _price;
        item.spr= item.spr = Resources.Load<Sprite>(_sprPath);
        return item;
    }

    public static Item Coby(Item item)
    {
        Item i = new Item();
        i.itemType = item.itemType;
        i.name = item.name;
        i.id = item.id;
        i.info = item.info;
        i.price = item.price;
        i.spr = item.spr;
        i.power = item.power;
        i.agility = item.agility;
        i.mentality = item.mentality;
        i.attack = item.attack;
        i.def = item.def;
        i.attackSpeed = item.attackSpeed;
        i.evade = item.evade;
        i.critical = item.critical;
        i.hp = item.hp;
        i.mp = item.mp;
        i.number = item.number;

        return i;
    }
}
