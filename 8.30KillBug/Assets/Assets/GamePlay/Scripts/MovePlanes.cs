using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlanes : MonoBehaviour {

    public Transform player; // 比较对象

    GameObject[] planes; // 地板集合
	// Use this for initialization
	void Start () {
        planes = GameObject.FindGameObjectsWithTag("Plane");
    }
	
	// Update is called once per frame
	void Update () {

        CheckMove();
    }


    //所有板子到中心点的距离都是一样的， 如果不一样就移动地板， 但是要往哪个方向，按照哪个坐标移动。
    //距离算出来之后也是一个向量， 然后计算距离的x 轴和 z轴 比原本时候的x z 轴 增大了多少， 如果z增大了，则只移动z轴， 如果x增大了或者只移动x轴就行了。
    //方法不行， 因为坐标有正  有负。 而且每个板子的坐标都不一样
    ////弄一个新的数组， 里面装载了所有边界地板当我移动时所有边界版按照固定位置摆放好， 而中心板不动。
    //public void CheckCentrePlane()       //检测中心板
    //{
    //    centre = GameObject.FindGameObjectWithTag("Centre");
    //}

    //public void Moveborderplanes()//加入所有边界地块进入到集合中
    //{
    //    planes = GameObject.FindGameObjectsWithTag("PeriPhery");
    //    Vector3 pos = centre.transform.position;
    //    //重置和移动所有边界版的位置  他们在中心点周围有一个固定的位置。      //实现了，但是还很低效
    //    planes[0].transform.position = new Vector3(pos.x - 50, 0, pos.z - 50);
    //    planes[1].transform.position = new Vector3(pos.x , 0, pos.z - 50);
    //    planes[2].transform.position = new Vector3(pos.x +50, 0, pos.z - 50);
    //    planes[3].transform.position = new Vector3(pos.x - 50, 0, pos.z );
    //    planes[4].transform.position = new Vector3(pos.x +50, 0, pos.z);
    //    planes[5].transform.position = new Vector3(pos.x - 50, 0, pos.z + 50);
    //    planes[6].transform.position = new Vector3(pos.x , 0, pos.z + 50);
    //    planes[7].transform.position = new Vector3(pos.x +50, 0, pos.z +50);
    //}


    void CheckMove()
    {
        for (int i = 0; i < planes.Length; i++)
        {
            Vector3 pos = planes[i].transform.position;
            if (player.position.x-pos.x<=75)
            {
                pos.x -= 150;
                planes[i].transform.position=pos;
            }
            if (player.position.z - pos.z <= 75)
            {
                pos.z -= 150;
                planes[i].transform.position = pos;
            }
            if (player.position.x - pos.x >= 75)
            {
                pos.x += 150;
                planes[i].transform.position = pos;
            }
            if (player.position.z - pos.z >= 75)
            {
                pos.z += 150;
                planes[i].transform.position = pos;
            }
        }
    }

}
