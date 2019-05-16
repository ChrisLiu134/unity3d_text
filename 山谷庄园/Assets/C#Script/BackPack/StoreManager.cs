using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 商店管理器
/// </summary>
public class StoreManager : MonoBehaviour {

    /// <summary>
    /// 所有商店格子用来存放商品数据
    /// </summary>
    [HideInInspector]
    public GameObject[] storeGrids;

    /// <summary>
    /// 小窗口显示物品信息的text
    /// </summary>
    public Text text;

    Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        storeGrids = GameObject.FindGameObjectsWithTag("Store").OrderBy(g=>g.transform.GetSiblingIndex()).ToArray();
       
    }

    // Use this for initialization
    void Start () {
        ResetStoreGrid();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 显示当前商店的所有商品
    /// </summary>
    public void ShowStoreItem()
    {
        Dictionary<int, Item> storeItems = ItemManager.Instance().StoreType();

        if (storeItems==null)
        {
            Debug.Log("此时附近没有商店");
            return;
        }
        int i = 0;
        foreach (var item in storeItems)
        {
            if (i>27)
            {
                break;
            }
            storeGrids[i].GetComponent<StoreItem>().GetItemInfo(item.Value);
            storeGrids[i].GetComponent<Image>().sprite = item.Value.spr;
            text.text = "";
            i++;
        }

    }

    /// <summary>
    /// 重置格子的数据和贴图
    /// </summary>
    public void ResetStoreGrid()
    {
        if (storeGrids == null)
        {
            Debug.Log("格子没有添加成功");
            return;
        }
        for (int i = 0; i < storeGrids.Length; i++)   //每次打开格子都将所有的格子数据重置为空
        {
            storeGrids[i].GetComponent<StoreItem>().GetItemInfo(new Item());
            storeGrids[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI1");
        }
    }

    /// <summary>
    /// 打开商店页面时的动画效果
    /// </summary>
    public void OpenShopWindow()
    {
        animator.SetTrigger("OpenShopWindow");
    }

}
