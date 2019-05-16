using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可以上下移动视角，以一定位置跟随主角的相机
/// </summary>
public class FollowCamera : MonoBehaviour
{

    /// <summary>
    /// 玩家目标
    /// </summary>
    Transform playerTarget;

    /// <summary>
    /// x 坐标移动速度
    /// </summary>
    public float xSpeed;

    /// <summary>
    /// y 坐标移动速度
    /// </summary>
    public float ySpeed;

    /// <summary>
    /// 相机最低高度
    /// </summary>
    public float yMinLimit;

    /// <summary>
    /// 相机最大高度 
    /// </summary>
    public float yMaxLimit;

    /// <summary>
    /// 滚动速度
    /// </summary>
    public float scrollSpeed;

    /// <summary>
    /// 摄像机距离玩家的最小值
    /// </summary>
    public float zoomMin;

    /// <summary>
    /// 摄像机距离玩家的最大值
    /// </summary>
    public float zoomMax;

    /// <summary>
    /// 相机和角色之间的相对距离
    /// </summary>
    float distance;

    /// <summary>
    /// 相机和角色之间的相对距离的线性差值，用于相机移动
    /// </summary>
    float distanceLerp;

    /// <summary>
    /// 临时坐标变量
    /// </summary>
    Vector3 position;

    /// <summary>
    /// 相机是否开始可以移动
    /// </summary>
    bool isActivated;

    /// <summary>
    /// 相机的欧拉角度 x的值
    /// </summary>
    float x;

    /// <summary>
    /// 相机的欧拉角度 y的值
    /// </summary>
    float y;

    // Use this for initialization
    void Start()
    {
        if (playerTarget == null)
        {
            playerTarget = FindObjectOfType<PlayerControl>().transform;
            if (playerTarget == null)
            {
                Debug.LogWarning("找不到玩家目标");
            }
        }

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        this.CalDistance();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ScrollMouse();
        RotateCamera();
    }

    /// <summary>
    /// 切换主控玩家
    /// </summary>
    public void ChangePlayer()
    {
        playerTarget = FindObjectOfType<PlayerControl>().transform;
        if (playerTarget == null)
        {
            Debug.LogWarning("找不到玩家目标");
        }
    }

    /// <summary>
    ///  设定相机开局以一个固定的角度望向玩家
    /// </summary>
    void CalDistance()
    {
        distance = zoomMax;           //相机离玩家的最大距离
        distanceLerp = distance;      
        Quaternion rotation = Quaternion.Euler(y, x, 0);     
        Vector3 calPos = new Vector3(0, 0, -distanceLerp);  // 计算向量
        position = rotation * calPos + playerTarget.position;  //设定坐标位相机的方向  以rotation为方向，走calpos的向量，加上玩家的位置 ，也就是相机的位置
        transform.rotation = rotation;  
        transform.position = position;
    }


    /// <summary>
    /// 按住鼠标右键 旋转鼠标调整视野
    /// </summary>
    public void RotateCamera()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isActivated = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isActivated = false;
        }

        if (playerTarget && isActivated)
        {
            if (playerTarget && isActivated)
            {
                //  相机的视角 和鼠标 移动的方向是想反的
                y -= Input.GetAxis("Mouse Y") * ySpeed;           
                x += Input.GetAxis("Mouse X") * xSpeed;

                //限制 y轴最大角度和 最低角度  避免相机沉入地面
                y = ClampAngle(y, yMinLimit, yMaxLimit);  

                Quaternion rotation = Quaternion.Euler(y, x, 0);
                Vector3 calPos = new Vector3(0, 0, -distanceLerp);
                position = rotation * calPos + playerTarget.position;
                transform.rotation = rotation;
                transform.position = position;
            }
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 calPos = new Vector3(0, 0, -distance);
            position = rotation * calPos + playerTarget.transform.position;
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    /// <summary>
    /// 滚动鼠标中间拉近和拉开视野
    /// </summary>
    void ScrollMouse()
    {
        distanceLerp = Mathf.Lerp(distanceLerp, distance, Time.deltaTime * 5);  
        if (Input.GetAxis("Mouse ScrollWheel")!=0 )
        {
            distance = Vector3.Distance(transform.position, playerTarget.transform.position);//计算相机和玩家之间的距离
            distance = ScrollLimit(distance - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, zoomMin, zoomMax);  //设定滚动相机的最大距离和最小距离
        }
    }

    /// <summary>
    /// 设定滚动鼠标视野的上下界限
    /// </summary>
    float ScrollLimit(float dist, float min, float max)
    {
        if (dist<min)
        {
            dist = min;
        }

        if (dist >max)
        {
            dist = max;
        }

        return dist;
    }


    /// <summary>
    /// 计算夹角
    /// </summary>
    float ClampAngle(float angle , float min, float max)
    {
        if (angle<-360)
        {
            angle += 360;
        }
        if (angle>360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }

}

