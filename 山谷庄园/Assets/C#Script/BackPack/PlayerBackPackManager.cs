using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBackPackManager {
    private static PlayerBackPackManager _insetance;
    public  static PlayerBackPackManager Instance()
    {
        if (_insetance==null)
        {
            _insetance = new PlayerBackPackManager();
        }
        return _insetance;
    }

    #region 装备栏合成系统
    /// <summary>
    /// 那当前背包的道具加上即将添加的道具， 与合成公式进行匹配。，如果成功了就返回这个合成公式，
    /// </summary>
    public ItemSynthesis CanSynthese(List<ItemSynthesis> itemSynthese, Item item, List<Item> backpack)
    {

        for (int i = 0; i < itemSynthese.Count; i++)
        {
            if (MatchingSynthesisFormula(backpack,item, itemSynthese[i]))
            {
                return itemSynthese[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 匹配合成公式，成功返回true。
    /// </summary>
    public bool MatchingSynthesisFormula(List<Item> backpack, Item item,ItemSynthesis itemSynthese)
    {
        int count = 0;

        List<int> idList = new List<int>();
        for (int i = 0; i < backpack.Count; i++)
        {
            idList.Add(backpack[i].id);
        }
        idList.Add(item.id);

        for (int i = 0; i < idList.Count; i++)
        {
            if (idList[i] == itemSynthese.id1)
            {
                idList.RemoveAt(i);
                count += 1;
                break;
            }
        }

        if (count==0)          
        {
            return false;
        }

        for (int i = 0; i < idList.Count; i++)
        {
            if (idList[i] == itemSynthese.id2)
            {
                idList.RemoveAt(i);
                count += 1;
                break;
            }
        }

        if (count == 1)
        {
            return false;
        }

        for (int i = 0; i < idList.Count; i++)
        {
            if (idList[i] == itemSynthese.id3)
            {
                idList.RemoveAt(i);
                count += 1;
                break;
            }
        }

        if (count == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
       
    }

    /// <summary>
    /// 按照参数里的合成公式进行合成
    /// </summary>
    /// <param name="currenplayer"></param>
    /// <param name="itemSyntheses"></param>
    public void Synthesis(PlayerControl currenplayer, ItemSynthesis itemSyntheses)
    {
        //先去掉合成所需的配方
        for (int i = 0; i < currenplayer.currenPlayerBackPack.Count; i++)
        {
            if (currenplayer.currenPlayerBackPack[i].id == itemSyntheses.id1)
            {
                currenplayer.LoseItem(currenplayer.currenPlayerBackPack[i]);
                break;
            }
        }

        for (int i = 0; i < currenplayer.currenPlayerBackPack.Count; i++)
        {
            if (currenplayer.currenPlayerBackPack[i].id == itemSyntheses.id2)
            {
                currenplayer.LoseItem(currenplayer.currenPlayerBackPack[i]);
                break;
            }
        }

        for (int i = 0; i < currenplayer.currenPlayerBackPack.Count; i++)
        {
            if (currenplayer.currenPlayerBackPack[i].id == itemSyntheses.id3)
            {
                currenplayer.LoseItem(currenplayer.currenPlayerBackPack[i]);
                break;
            }
        }
        //得到合成后的新装备
        currenplayer.GetItem(ItemManager.Instance().EQItemsShop[itemSyntheses.id]);

        //检测是不是可以继续合成
        currenplayer.ISCanSynthesized(new Item());
    }
    #endregion
}
