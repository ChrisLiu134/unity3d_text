using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

/// <summary>
/// 掉落在地上的道具的脚本。
/// </summary>
public class DropItem : MonoBehaviour {
    public Item dropItem = new Item();

    /// <summary>
    /// 显示信息面板的窗口
    /// </summary>
    public GameObject itemInfoView;

    /// <summary>
    /// 显示物品的信息
    /// </summary>
    public Text text;
	// Use this for initialization
	void Start () {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// 鼠标指向当前掉落物品时， 打开玩家的物品信息栏显示这个道具的信息
    /// </summary>
    public void ShowDropItemInfo()
    {
        if (dropItem==null)
        {
            Debug.Log("当前物体上找不到道具");
            return;
        }
        itemInfoView.SetActive(true);
        text.text = dropItem.name;
    }

    /// <summary>
    /// 关闭物品信息栏
    /// </summary>
    public void CloseItemInfoView()
    {
        itemInfoView.SetActive(false);
        text.text = "";
    }
}
