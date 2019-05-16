using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillStateGrid : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
    public SkillState skillState=new SkillState();

    /// <summary>
    /// 显示info的信息栏
    /// </summary>
    public Text text;

    public void Update()
    {
        Close();
    }

    /// <summary>
    /// 如果状态不再了，关闭这一状态栏
    /// </summary>
    public void Close()
    {
        if (!skillState.haveState)
        {
            text.text = "";
            gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.text = skillState.info;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.text = "";
    }
}
