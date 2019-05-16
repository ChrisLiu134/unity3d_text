using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

/// <summary>
/// 玩家身上的道具脚本
/// </summary>
public class PlayerBackpackItem : MonoBehaviour,IPointerExitHandler,IPointerClickHandler,IDragHandler,IBeginDragHandler,IEndDragHandler{
    Item playerItem = new Item();

    /// <summary>
    /// 装备栏信息小窗口
    /// </summary>
    public GameObject itemInfoView;

    /// <summary>
    /// 道具的详细信息介绍
    /// </summary>
    public Text text;

    /// <summary>
    /// 当前使用道具的人
    /// </summary>
    PlayerControl playerControl;

    /// <summary>
    /// 拖拽用的组件
    /// </summary>
    public GameObject dragObj_Prefeb;

    /// <summary>
    /// 拖拽中的物品
    /// </summary>
    GameObject dragObj;

    /// <summary>
    /// 当前玩家背包
    /// </summary>
    List<Item> playerItemUser_BackPack;

    /// <summary>
    /// 掉落的道具的预制体
    /// </summary>
    public GameObject dropItem_Prefeb;

    UIManager uIManager;

    // Use this for initialization
    void Start () {
        uIManager = FindObjectOfType<UIManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetItemInfo(Item item)
    {
         playerItem = item;
    }

    #region   点击事件， 拖拽事件， 鼠标指向事件， 等等。
    /// <summary>
    /// 鼠标移到装备栏的道具上时，触发这个事件，显示装备的信息
    /// </summary>
    public void ShowItemInfo()
    {
        itemInfoView.SetActive(true);
        text.text = playerItem.info;
    }

    /// <summary>
    /// 鼠标离开装备栏的道具上时，触发这个事件，关掉信息窗口
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoView.SetActive(false);
        text.text = "";

    }

    /// <summary>
    /// 点击当前道具时触发的事件。 如果是药品就喝药
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (playerItem == null)
        {
            Debug.Log("当前道具栏为空");
            return;
        }
        if (playerItem.itemType == Item.ItemType.ME)
        {
            IdentifyCurrentPlayer();

            if (playerItem.hp>0)
            {
                if (playerControl.playerCha.player.hp != playerControl.playerCha.player.maxHp)
                {
                    playerControl.playerCha.player.hp += playerItem.hp;
                    playerItem.number -= 1;
                }
            }
            else if (playerItem.mp > 0)
            {
                if (playerControl.playerCha.player.mp != playerControl.playerCha.player.maxMp)
                {
                    playerControl.playerCha.player.mp += playerItem.mp;
                    playerItem.number -= 1;
                }
            }
            if (playerItem.number <= 0)
            {
                playerControl.LoseItem(playerItem);
            }
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 开始拖拽的时候
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (playerItem.id==0)                  //空格子不可以被拖拽
        {
            Debug.Log("这个被拖拽的格子上面没有物品");
            return;
        }
        //开始拖拽的时候,生成一个物体， 物体的数据和被拖拽的道具的数据一样，
        dragObj = Instantiate(dragObj_Prefeb, GameObject.Find("Canvas").transform);
        dragObj.GetComponent<Image>().sprite = playerItem.spr;
        dragObj.GetComponent<DragItem>().dragItem=playerItem;
        DragObjFollowMouse(eventData);

        //然后重置被拖拽的这个格子对应玩家背包里这个道具的数据


        if (!IdentifyCurrentPlayer())
        {
            Debug.Log("没找到控制者无法操作");
            return;
        }

        for (int i = 0; i < uIManager.BackPackGrids.Length; i++)
        {
            if (string.Equals(uIManager.BackPackGrids[i].name, transform.name))
            {
                playerItemUser_BackPack[i] = new Item();
                dragObj.GetComponent<DragItem>().subscript = i;
            }
        }
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        DragObjFollowMouse(eventData);
    }

    /// <summary>
    /// 结束拖拽时
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IdentifyCurrentPlayer())
        {
            Debug.Log("没找到控制者无法操作");
            return;
        }

        if (dragObj!=null)
        {
            if (eventData.pointerEnter != null)  // 意思是物品结束拖拽的点还在ui上
            {
                 if (eventData.pointerEnter.CompareTag("Store"))  //如果当前物品被拖拽到了商店界面上
                {
                    playerControl.playerCha.player.money += dragObj.GetComponent<DragItem>().dragItem.price / 2;
                }
                else if (eventData.pointerEnter!=transform && eventData.pointerEnter.GetComponent<PlayerBackpackItem>()!=null)
                {
                    for (int i = 0; i < uIManager.BackPackGrids.Length; i++)
                    {
                        if (string.Equals(uIManager.BackPackGrids[i].name, eventData.pointerEnter.name))
                        {
                            // 拖拽物体上对应背包当前下标的的item数据修改为当前物体上的item数据。
                            playerItemUser_BackPack[dragObj.GetComponent<DragItem>().subscript] = eventData.pointerEnter.GetComponent<PlayerBackpackItem>().playerItem;
                            //这个格子所对应的玩家背包的item数据修改成拖拽到它身上的物体上的item的数据， 因为我的格子的item数据是根据玩家背包的item的顺序来显示的。
                            playerItemUser_BackPack[i] = dragObj.GetComponent<DragItem>().dragItem;
                        }
                    }
                }
                else
                {
                    playerItemUser_BackPack[dragObj.GetComponent<DragItem>().subscript] = dragObj.GetComponent<DragItem>().dragItem;
                }
            }
            else   //如果不在了，表示玩家丢掉当前装备，  玩家的背包要去掉当前装备，地上要生成一个物体表示这个装备。
            {
                //计算当前鼠标停留的位置
                RaycastHit hit = playerControl.MousePointer();
                Vector3 pos = new Vector3(hit.point.x * 1, hit.point.y * 1, hit.point.z * 1);
                //

                //物品丢弃时掉落的位置为鼠标指向的位置。
                GameObject obj = Instantiate(dropItem_Prefeb, pos, Quaternion.identity);
                obj.GetComponent<DropItem>().dropItem = dragObj.GetComponent<DragItem>().dragItem;

                //当前控制的角色失去当前装备
                eventData.pointerDrag.GetComponent<PlayerBackpackItem>().playerControl.playerCha.PlayerloseItemAttribute(dragObj.GetComponent<DragItem>().dragItem);
                eventData.pointerDrag.GetComponent<PlayerBackpackItem>().playerItem = new Item();
            }
            Destroy(dragObj); //删除临时储存数据的物体
        }
    }

    #endregion

    /// <summary>
    /// 拖拽中的组件跟随鼠标位置移动
    /// </summary>
    public void DragObjFollowMouse(PointerEventData eventData)
    {
        if (dragObj==null)
        {
            Debug.Log("预制体加载失败，没有生成拖拽中的物体");
            return;
        }
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(dragObj.GetComponent<RectTransform>(), eventData.position,
        eventData.pressEventCamera, out globalMousePos))
        {
            dragObj.GetComponent<RectTransform>().position = globalMousePos;
        }
    }

    /// <summary>
    /// 确认当前玩家的主控角色和背包数据
    /// </summary>
    public bool IdentifyCurrentPlayer()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        if (playerControl == null)
        {
            Debug.Log("没找到当前控制的角色");
            return false;
        }
        playerItemUser_BackPack = new List<Item>();
        if (playerControl.transform.name == "Swordman")
        {
            playerItemUser_BackPack = ItemManager.Instance().swordmanBackPack;
        }
        else if (playerControl.transform.name == "Magician")
        {
            playerItemUser_BackPack = ItemManager.Instance().magicianBackPack;
        }
        return true;
    }

}


