using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 让ui遮挡射线 
/// </summary>
public class ShadeRay : MonoBehaviour {

    GraphicRaycaster raycaster;
    EventSystem eventSystem;
    // Use this for initialization
    void Start () {
        raycaster = GetComponent<GraphicRaycaster>();
	}
	
    /// <summary>
    /// 如果点击到了ui层的物体
    /// </summary>
    /// <returns></returns>
    public bool CheckUiRayCastObjects()
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        raycaster.Raycast(eventData, list);

        return list.Count > 0;


    }
}
