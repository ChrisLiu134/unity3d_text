using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMouse : MonoBehaviour {
    //不同的鼠标贴图
    public Texture2D cursora;
    public Texture2D cursorb;
    public Texture2D oldCurs;
    public Texture2D itemCursor;
    public Texture2D useSkillCursor;
    //


    // Use this for initialization
    void Start () {
        Cursor.SetCursor(oldCurs, Vector2.zero, CursorMode.ForceSoftware);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    /// <summary>
    /// 切换鼠标显示
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="isUseSkill"></param>
    public void Change(RaycastHit hit,bool isUseSkill)
    {
        if (isUseSkill)
        {
            Cursor.SetCursor(useSkillCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
        else
        {
            if (hit.transform != null)
            {
                if (hit.transform.CompareTag("Monster"))
                {
                    Cursor.SetCursor(cursora, Vector2.zero, CursorMode.ForceSoftware);
                }
                else if (hit.transform.CompareTag("Npc"))
                {
                    Cursor.SetCursor(cursorb, Vector2.zero, CursorMode.ForceSoftware);
                }
                else if (hit.transform.CompareTag("DropItem"))
                {
                    hit.transform.GetComponent<DropItem>().ShowDropItemInfo();
                    Cursor.SetCursor(itemCursor, Vector2.zero, CursorMode.ForceSoftware);
                }
                else
                {
                    Cursor.SetCursor(oldCurs, Vector2.zero, CursorMode.ForceSoftware);
                }
            }
        }
    }
}
