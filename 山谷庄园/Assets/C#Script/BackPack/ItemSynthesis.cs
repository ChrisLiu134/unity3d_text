using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



//合成系统
/*
1.   水晶剑 id：30  = 阔剑 id：13 + 大剑 id：15  +水晶剑合成卷轴 id：29
2.   蝴蝶   id：31   = 鹰角弓 id：22 +闪避护符 id：21  +短棍 id：14
3.   散夜对剑 id：32 = 夜叉 id：34 + 散华 id：47+小圆环 id：2
4.   夜叉   id：34  = 欢欣之刃 id：11 + 精灵皮鞋 id：8 + 夜叉合成卷轴 id：33
5.   暗灭   id：36  = 秘银锤 id：16 + 秘银锤 id：16 + 暗灭合成卷轴 id：35
6.   辉耀   id：38  = 圣者遗物 id：24 + 圣者遗物 id：24 + 辉耀合成卷轴 id：37
7.   先锋盾 id：39  = 生命之球 id：28 +圆盾 id：17+圆盾 id：17
8.   龙芯   id：41  = 生命之球 id：28 + 希梅斯特的掠夺 id：23 +龙芯合成卷轴 id：40
9.   强袭   id: 43  = 先锋盾 id：39 +板甲 id：19 + 强袭合成卷轴 id：42
10.  冰霜护甲 id：45 =神秘法杖 id：25 +板甲 id：19 + 冰霜护甲卷轴 id：44
11.  散华   id：47 = 力量腰带 id：7+ 食人魔之斧 id：10+ 散华卷轴 id：46
*/


/// <summary>
/// 装备合成公式类
/// </summary>
public class ItemSynthesis  {
    //由id3 + id2 +id1 合成id
    public int id;
    public int id1;
    public int id2;
    public int id3;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_id1"></param>
    /// <param name="_id2"></param>
    /// <param name="_id3"></param>
    public ItemSynthesis(int _id, int _id1,int _id2,int _id3)
    {
        id = _id;
        id1 = _id1;
        id2 = _id2;
        id3 = _id3;
    }

}
