using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;



     

public class GameMap : MonoBehaviour {
    enum EGameState
    {
        Setpoint,          //开始状态等待
        StartCalculation,      //开始寻找
        Calculation,      //寻找中
        ShowPath,         //显示路径
        Finish,            //完成
    }

    enum  SearchWay //搜索方式的枚举
    {
        BFS,
        DFS,
        ASTAR,
        Link,
    }




    public Transform cube;
    public Transform startCubes;
    public Transform endCubes;
    public Transform wall;
    public Transform father;
    EGameState gameState;
    int[,] map;
    int[,] step;         
    int W = 30;
    int H = 20;

    const int END = 10;
    const int START = 9;
    const int CANMOVE = 0;
    const int WALL = 1;
    List<Pos> lp;
    Pos startPos;
    Pos endPos;

    // Use this for initialization
    void Start () {
        gameState = EGameState.Setpoint;
        map = new int[H, W];          //对应的是地图上的内容。
        step = new int[H, W];        //对应的内容是步数
        lp = new List<Pos>();
        ReadMapFile();
        InitMap();
        
    }
	
	// Update is called once per frame
	void Update () {
       switch(gameState)
        {
            case  EGameState.Setpoint:
                SetPoint();
                break;
            case EGameState.StartCalculation:
                FindRoad();
                break;
            case EGameState.ShowPath:
                break;
            case EGameState.Finish:
                break;
        }
    }

    int n = 0;//控制起点终点设置
    void SetPoint()//然后鼠标点击生成起点 终点 。
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hitt = new RaycastHit();        
            Physics.Raycast(ray, out hitt);
            if (hitt.transform != null && n == 1)
            {
                int x = (int)hitt.point.x;
                int z = (int)hitt.point.z;
                endCubes.position = new Vector3(x, 0.5f, z);
                endPos = new Pos(x, z);
                map[z, x] = END;

                gameState = EGameState.StartCalculation;
            }
            if (hitt.transform!=null&&n==0)
            {
                int x = (int)hitt.point.x;
                int z = (int)hitt.point.z;
                startCubes.position = new Vector3(x, 0.5f, z);
                startPos = new Pos(x, z);
                map[z, x] = START;
                n += 1;
            }
        }
    }
   
    void ReadMapFile()// 读取指定路径的txt文件， 然后 将数据存入数组中 。
    {
        string path = Application.dataPath  + "//Maps" + "//" + "map.txt";
        if (!File.Exists(path))
        {
            return;
        }
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //打开文件用于读取
        StreamReader read = new StreamReader(fs, Encoding.Default); // 读取方式

        string strReadLine = "";  //读取内容保存在字符串中。
        int z = 0;
        read.ReadLine(); //跳过第一行
        strReadLine = read.ReadLine();  //读取的时候可以一行一行的往下读取
        while (strReadLine!=null && z<H)
        {
            for (int x = 0; x < W; x++)
            {
                int n; 
                switch(strReadLine[x])            //以读取的文字的字符为选择
                {
                    case '1':
                        n = WALL;
                        break;
                    default:
                        n = CANMOVE;
                        break;                       
                }
                map[z,x] = n;
            }
            z += 1;            
            strReadLine = read.ReadLine();
        }
        read.Dispose(); //文件流释放
        fs.Close();
    }

    void InitMap()  //读取数组中的数据生成墙
    {
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (map[i,j]== WALL)
                {
                    Instantiate(wall, new Vector3(j * 1, 0.5f, i * 1), Quaternion.identity, father);
                }
            }
        }
    }

    //开始寻找
    void FindRoad()
    {
        for (int i = 0; i < H; i++)                              //先设置点数 来限制添加步数， 然后再去生成
        {
            for (int j = 0; j < W; j++)
            {
                step[i, j] = int.MaxValue;
            }
        }

        step[startPos.z, startPos.x] = 0;
        
        lp.Add(startPos);

        while (lp.Count>0)
        {
            Pos newpos = lp[lp.Count-1];
            lp.RemoveAt(lp.Count-1);

            //上
            if (newpos.z<H-1)
            {
                //Debug.Log("上");
                if (Find(newpos, 0, 1))
                {
                    break;
                }
            }

            //下
            if (newpos.z>0)
            {
                //Debug.Log("下");
                if (Find(newpos, 0, -1))
                {
                    break;
                }
            }

            //左
            if (newpos.x>0)
            {
                //Debug.Log("左");
                if (Find(newpos, -1, 0))
                {
                    break;
                }
            }

            //右
            if (newpos.x<W-1)
            {
                //Debug.Log("右" );
                if (Find(newpos, 1, 0))
                {
                    break;
                }
            }
        }
    }


    bool Find(Pos p , int _x , int _z)
    {
        //如果这一步到终点了
        if (map[p.z+_z,p.x+_x]== END)  //如果这个点是终点的点了。
        {
            Debug.Log("找到终点了");
            step[p.z + _z, p.x + _x] = step[p.z, p.x] + 1;  //终点的步数是上一步加1  便于后期返回路径
            gameState = EGameState.ShowPath;
            return true;
        }

        if (map[p.z+_z,p.x+_x]== CANMOVE)            //首先要可以走 不是墙 1  和起点终点 9 10
        {
            //Debug.Log(string.Format("HH {0} {1} x:{2}_{3} z:{4}_{5}", step[p.z + _z, p.x + _x], step[p.z, p.x], p.x, _x, p.z, _z));
            if (step[p.z +_z, p.x +_x]>step[p.z,p.x]+1)        //步数比前一步要大  然后这个坐标的步数就等于前一步加1步  
            {
                step[p.z + _z,p.x + _x] = step[p.z, p.x] + 1;
                lp.Add(new Pos (p.x + _x, p.z + _z));
                //Debug.Log("step[p.z + _z, p.x + _x] =====" + step[p.z + _z, p.x + _x]+ "=====step[p.z, p.x]====="+ step[p.z, p.x]);
                Transform g = Instantiate(cube, new Vector3(p.x * 1, 0.5f, p.z * 1), Quaternion.identity, father);
                g.Find("number").GetComponent<TextMesh>().text = step[p.z + _z, p.x + _x].ToString();
            }
        }
        return false;
    }



    

    //不太完美的方式
    void haha()
    {
        // 把起点设置为 startpos          首先 把一个pos 上下左右一次。 生成四个新的。  然后

        //初版正确方向尝试
        //while (t<10)
        //{
        //    for (int i = 0; i < lp.Count; i++)
        //    {
        //        newPos_1 = new Pos(lp[i].x + t, lp[i].z);
        //        newPos_2 = new Pos(lp[i].x - t, lp[i].z);
        //        newPos_3 = new Pos(lp[i].x, lp[i].z + t);
        //        newPos_4 = new Pos(lp[i].x, lp[i].z - t);
        //        Instantiate(cube, new Vector3(newPos_1.x, 0.5f, newPos_1.z), Quaternion.identity, father);
        //        Instantiate(cube, new Vector3(newPos_2.x, 0.5f, newPos_2.z), Quaternion.identity, father);
        //        Instantiate(cube, new Vector3(newPos_3.x, 0.5f, newPos_3.z), Quaternion.identity, father);
        //        Instantiate(cube, new Vector3(newPos_4.x, 0.5f, newPos_4.z), Quaternion.identity, father);

        //        lp.Remove(lp[i]);
        //    }
        //    lp.Add(newPos_1);
        //    lp.Add(newPos_2);
        //    lp.Add(newPos_3);
        //    lp.Add(newPos_4);
        //    t++;
        //}      //首先通过 list  存步数  然后通过步数生成新的要走的列表， 然后再生成新的步数和走的位置。
        
        //while (bo == false)
        //{
        //    for (int i = 0; i < lp.Count; i++)
        //    {
        //        if (lp[i].z + 1 < W - 1)
        //        {
        //                step[lp[i].x,lp[i].z + 1] = t;
        //                if (lp[i].x == endPos.x && lp[i].z + 1 == endPos.z)
        //                {
        //                    bo = true;
        //                }

        //        }

        //        if (lp[i].z - 1 > 1)
        //        {
                    
        //                step[lp[i].x,lp[i].z - 1] = t;
        //                if (lp[i].x == endPos.x && lp[i].z - 1 == endPos.z)
        //                {
        //                    bo = true;
        //                }
                    
        //        }
        //        if (lp[i].x - 1 > 1)
        //        {
                    
        //                step[lp[i].x - 1,lp[i].z] = t;
        //                if (lp[i].x + 1 == endPos.x && lp[i].z == endPos.z)
        //                {
        //                    bo = true;
        //                }
                    
        //        }

        //        if (lp[i].x + 1 < H - 1)
        //        {
                    
        //                step[lp[i].x + 1,lp[i].z] = t;
        //                if (lp[i].x - 1 == endPos.x && lp[i].z == endPos.z)
        //                {
        //                    bo = true;
        //                }
        //        }
        //    }
        //    lp.Clear();
        //    for (int i = 0; i < step.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < step.GetLength(1); j++)
        //        {
        //            if (step[i, j] == t)
        //            {
        //                Pos newPos = new Pos(i, j);
        //                lp.Add(newPos);
        //                if (map[i,j]!=1)
        //                {
        //                    Instantiate(cube, new Vector3(i, 0.5f, j), Quaternion.identity);
        //                }
        //            }
        //        }
        //        yield return null;
        //    }
        //    t++;
        //}

        //试试扩展一个， 然后删除掉，然后再拓展下一个 
        //while (a<10||b<10)
        //{
        //    Instantiate(cube, new Vector3(startPos.x + a, 0.5f, startPos.z),Quaternion.identity);

        //    Instantiate(cube, new Vector3(startPos.x -a, 0.5f, startPos.z), Quaternion.identity);

        //    Instantiate(cube, new Vector3(startPos.x , 0.5f, startPos.z+b), Quaternion.identity);

        //    Instantiate(cube, new Vector3(startPos.x , 0.5f, startPos.z-b), Quaternion.identity);

        //    a++;
        //    b++;
        //    
        //}
    }

    void CreatPath()     //生成查找的方块
    {
        //for (int i = 0; i < H; i++)                              //先设置点数 来限制添加步数， 然后再去生成
        //{
        //    for (int j = 0; j < W; j++)
        //    {
        //        if (map[i, j] == 0 && step[i, j] != int.MaxValue)
        //        {

        //        }
        //    }
        //}
    }

}
