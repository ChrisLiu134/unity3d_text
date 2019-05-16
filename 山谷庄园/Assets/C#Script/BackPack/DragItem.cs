using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
/// <summary>
/// 拖拽中的物体上的脚本
/// </summary>
public class DragItem : MonoBehaviour
{

    public Item dragItem = new Item();

    /// <summary>
    /// 当前道具在背包中的下标位置
    /// </summary>
    public int subscript;

}
