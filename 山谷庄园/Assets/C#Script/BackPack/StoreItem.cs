using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
/// <summary>
/// 商店格子里的道具的信息
/// </summary>
public class StoreItem : MonoBehaviour {

    Item storeItem = new Item();

    /// <summary>
    /// 商品小窗口信息展示
    /// </summary>
    public Text text;

    /// <summary>
    /// 当前玩家操控的角色
    /// </summary>
    PlayerControl currenPlayerContol;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 获取道具信息给当前格子
    /// </summary>
    public void GetItemInfo(Item item)
    {
        storeItem = item;
    }

    /// <summary>
    /// 鼠标指向事件
    /// </summary>
    public void ShowItemInfo()
    {
        if (storeItem==null)
        {
            return;
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("名字 ：" + storeItem.name+"\n");
        sb.Append("简介 ：" + storeItem.info + "\n");
        sb.Append("价格 ：" + storeItem.price);
        text.text = sb.ToString();
    }
    /// <summary>
    /// button事件买装备
    /// </summary>
    public void PlayerBuyItem()
    {
        currenPlayerContol = FindObjectOfType<PlayerControl>();
        //先确认当前使用者
        if (currenPlayerContol==null)
        {
            Debug.Log("没找到控制者无法操作");
            return;
        }
        if (storeItem==null)
        {
            Debug.Log("这个格子里没有物品");
            return;
        }
        if (currenPlayerContol.playerCha.player.money < storeItem.price)
        {
            Debug.Log("钱不够购买当前物品");
            return;
        }
        switch (storeItem.itemType)            //先分类
        {
            case Item.ItemType.EQ:
                if (currenPlayerContol.ISCanSynthesized(storeItem))
                {
                    currenPlayerContol.playerCha.player.money -= storeItem.price;
                    return;   
                }
                if (currenPlayerContol.GetItem(storeItem))
                {
                    currenPlayerContol.playerCha.player.money -= storeItem.price;
                }
                break;
            case Item.ItemType.ME:
                Item temp = Item.Coby(storeItem);
                //购买药品时，需要先遍历一遍当前购买者背包中的物品是不是没有重复，如果有就得叠加并且购买成功.任何结束当前函数。
                for (int i = 0; i < currenPlayerContol.currenPlayerBackPack.Count; i++)   
                {
                    if (currenPlayerContol.currenPlayerBackPack[i].id== temp.id && currenPlayerContol.currenPlayerBackPack[i].id!=0)
                    {
                        if (currenPlayerContol.currenPlayerBackPack[i].number==99)
                        {
                            Debug.Log("到99上限了");
                            break;
                        }
                        currenPlayerContol.currenPlayerBackPack[i].number += temp.number; ;
                        currenPlayerContol.playerCha.player.money -= temp.price;
                        return;
                    }
                }
               
                if (currenPlayerContol.GetItem(temp))
                {
                    currenPlayerContol.playerCha.player.money -= storeItem.price;
                }
                break;
            case Item.ItemType.RB:
                if (currenPlayerContol.ISCanSynthesized(storeItem))
                {
                    currenPlayerContol.playerCha.player.money -= storeItem.price;
                    return;
                }
                if (currenPlayerContol.GetItem(storeItem))
                {
                    currenPlayerContol.playerCha.player.money -= storeItem.price;
                }
                break;
        }
    }
}
