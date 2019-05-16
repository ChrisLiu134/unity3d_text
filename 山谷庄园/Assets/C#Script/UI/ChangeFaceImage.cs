using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFaceImage : MonoBehaviour {
    /// <summary>
    /// 面部相机的预制体
    /// </summary>
    public GameObject faceCamera_Prefab;

    /// <summary>
    /// 当前已有的面部相机
    /// </summary>
    public GameObject currenFaceCamera;

    /// <summary>
    /// 绿色光圈特效， 鼠标右键指谁就显示在谁的身上，并且信息面部显示该物体的绝色信息
    /// </summary>
    public GameObject greenCircle_Prefab;

    /// <summary>
    /// 当前场景已经存在的绿圈
    /// </summary>
    public GameObject currenGreenCircle;

    /// <summary>
    /// 当前选定的物体
    /// </summary>
    public GameObject currenGameObject;

    UIManager uIManager;
    /// <summary>
    ///  当前物体的层级
    /// </summary>
    public int layer;

    // Use this for initialization
    void Start () {
        currenFaceCamera = GameObject.FindGameObjectWithTag("FaceCamera");
        currenGreenCircle= GameObject.FindGameObjectWithTag("GreenCircle");
        uIManager = FindObjectOfType<UIManager>();
        currenGameObject = currenFaceCamera.transform.parent.gameObject;
        ChangeLayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 点击鼠标右键的事件
    /// </summary>
    public void InputMouseOne(RaycastHit hit)
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (hit.transform.CompareTag("Monster") || hit.transform.CompareTag("Npc") || hit.transform.CompareTag("Player"))
            {
                ChangeFace(hit.transform);
                uIManager.currenObj = hit.transform.gameObject;
            }
        }
    }

    /// <summary>
    /// 更换当前查看的物体的所有子物体的层级，好在脸部相机上面单独显示他，但是需要注意不要换掉了小地图的层级
    /// </summary>
    public void ChangeLayer()
    {
      
        foreach (Transform trans in currenGameObject.transform)
        {
            if (trans.gameObject.layer!=12 && trans.gameObject.layer!=0)
            {
                trans.gameObject.layer = 15;
            }
            layer = currenGameObject.layer;
        }
    }

    /// <summary>
    /// 切换了物体之后，把之前物体的层级返回原先的层级
    /// </summary>
    public void ReturnLyaer()
    {
        foreach (Transform trans in currenGameObject.transform)
        {
            if (trans.gameObject.layer != 12 && trans.gameObject.layer != 0)
            {
               trans.gameObject.layer = layer;
            }
        }
    }

    /// <summary>
    /// 切换面部摄像头和光圈到指定的物体上
    /// </summary>
    /// <param name="trans"></param>
    public void ChangeFace(Transform trans)
    {
        //先消除掉上一个物体的相机和绿圈以及layer层级
        ReturnLyaer();
        Destroy(currenFaceCamera);
        Destroy(currenGreenCircle);
        
        //更换新的物体采取的操作
        currenGameObject = trans.gameObject;
        ChangeLayer();
        GameObject face = Instantiate(faceCamera_Prefab, trans);
        currenFaceCamera = face;
        GameObject green= Instantiate(greenCircle_Prefab, trans);
        currenGreenCircle = green;
    }
}
