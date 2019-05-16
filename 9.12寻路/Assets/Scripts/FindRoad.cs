using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Pos     //自己设置pos来代替vector2  代表位置 整数
{
    public int x = 0;
    public int z = 0;

    public Pos()
    {

    }

    public Pos(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    /// <summary>
    /// 计算两个pos坐标点的距离
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static float AStarDistance(Pos p1, Pos p2)
    {
        float d1 = Mathf.Abs(p1.x - p2.x);
        float d2 = Mathf.Abs(p1.z - p2.z);
        return d1 + d2;
    }

    public bool Equals(Pos p)
    {
        return x == p.x && z == p.z;
    }
}

public class AScore
{
    public float G = 0;   //代表  G＝目标离自己的距离
    public float H = 0;    //H＝移动距离
    public bool closed = false;         //关闭掉一个点的移动能力。
    public Pos parent = null;            //ascore类的对象都有一个属于自己的pos类的对象

    public AScore(float _g, float _h)       
    {
        G = _g;
        H = _h;
        closed = false;
    }

    public float F         //新的属性 f 等于 g+h的值  没有字段    
    {
        get { return G + H; }
    }

    public int CompareTo(AScore a2)        //比较两者的f的值
    {
        if (F==a2.F)
        {
            return 0;
        }
        if (F>a2.F)
        {
            return 1;
        }
        return -1;
    }

    public bool Equals(AScore a)           //让这个参数里的ascoref的值 等于另外一个ascore的f 如果等于就返回true  如果不等于就返回false
    {
        if (a.F == F)
        {
            return true;
        }
        return false;
    }
}


public class FindRoad : MonoBehaviour {
    int W = 30;
    int H = 30;
    int depth=0;

    int[,] map;
    int[,] step;

    public GameObject prefab_wall;
    public GameObject prefeb_start;
    public GameObject prefab_end;
    public GameObject prefab_path;     //路径预制体
    public Transform prefab_father;

    Pos startPos;
    Pos endPos;


    public enum SearchWay //搜索方式的枚举
    {
        BFS,
        DFS,
        ASTAR,
        Link,
    }

    public SearchWay searchWay;


    const int STARTCube = 8;  //常量
    const int ENDCube = 9;
    const int WALL = 1;

    enum EGameState
    {
        SetBeginpoint,          //设置开始点
        SetEndPoint,          //设置终点
        StartCalculation,      //开始寻找
        Calculation,      //寻找中
        ShowPath,         //显示路径
        Finish,            //完成
    }

    EGameState gameState = EGameState.SetBeginpoint;


	// Use this for initialization
	void Start () {
        //pathParent = GameObject.Find("PathParent");     //通过名字找到组件
        map = new int[H, W];
        step = new int[H, W];
        ReadMapFile();        
        InitMap();
    }
	
	// Update is called once per frame
	void Update () {
		switch(gameState)
        {
            case EGameState.SetBeginpoint:
                if (SetPoint(STARTCube))
                {
                    gameState = EGameState.SetEndPoint;
                }
                break;
            case EGameState.SetEndPoint:
                if (SetPoint(ENDCube))
                {
                    gameState = EGameState.StartCalculation;
                }
                break;
            case EGameState.StartCalculation:
                switch(searchWay)
                {
                    case SearchWay.BFS:
                        StartCoroutine(BFS_DFS());
                        break;
                    case SearchWay.DFS:
                        StartCoroutine(BFS_DFS());
                        break;
                    case SearchWay.ASTAR:
                        StartCoroutine(AStart());
                        break;
                }
                //FindR();
                gameState = EGameState.Calculation;
                break;
            case EGameState.ShowPath:
                switch (searchWay)
                {
                    case SearchWay.BFS:
                        BFSShowPath();
                        break;
                    case SearchWay.DFS:
                        DFSShowPath();
                        break;
                    case SearchWay.ASTAR:
                        AStartShowPath();
                        break;
                }
                gameState = EGameState.Finish;
                break;
            case EGameState.Finish:
                break;
        }
	}


    void ReadMapFile()  //读取地图文件
    {
        string path = Application.dataPath + "//Maps" + "//" + "map.txt"; ;
        if (!File.Exists(path))
        {
            return;
        }

        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader read = new StreamReader(fs, Encoding.Default);

        string strReadline = "";
        int z = 0;

        // 跳过第一行
        read.ReadLine();
        strReadline = read.ReadLine();

        while (strReadline != null && z < H)
        {
            for (int x = 0; x < W && x < strReadline.Length; ++x)
            {
                int t;
                switch (strReadline[x])
                {
                    case '1':
                        t = 1;
                        break;
                    case '8':
                        t = 8;
                        break;
                    case '9':
                        t = 9;
                        break;
                    default:
                        t = 0;
                        break;
                }
                //                Debug.Log("x, y"+ x +" " + y);
                map[z, x] = t;
            }
            z += 1;
            strReadline = read.ReadLine();
        }

        read.Dispose();//文件流释放  
        fs.Close();
    }

    void InitMap()       //按照文件生成地图
    {
        GameObject walls = new GameObject();    //代码生成一个父类    
        walls.name = "walls";

        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (map[i, j] == WALL)
                {
                    GameObject w = Instantiate(prefab_wall, new Vector3(j * 1, 0.5f, i * 1), Quaternion.identity, prefab_father);
                    w.transform.parent = walls.transform;
                }
            }
        }

        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                step[i, j] = int.MaxValue;
            }
        }
    }

    bool SetPoint(int n)      //设置起点终点
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();
            Physics.Raycast(ray, out hit);
            if (hit.transform!=null)
            {
                int x = (int)hit.point.x;
                int z = (int)hit.point.z;

                map[z, x] = n;

                if (n==STARTCube)
                {
                    startPos = new Pos(x, z);
                    prefeb_start.transform.position = new Vector3(x * 1, 0.5f, z* 1);
                }
                if (n==ENDCube)
                {
                    endPos = new Pos(x, z);
                    prefab_end.transform.position = new Vector3(x * 1, 0.5f, z * 1);
                }
                return true;
            }
        }
        return false;
    }

    List<Pos> listPos = new List<Pos>();

    IEnumerator BFS_DFS()         //开始bfs查找
    {
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                step[i, j] = int.MaxValue;
            }
        }

        step[startPos.z, startPos.x] = 0;
        listPos.Add(startPos);
        Pos p=new Pos();
        //主要的探索循环， 首先拿list中第一个点 来上下左右探寻，  然后将每个新的点都存入到list中， 然后继续探索，直到找到终点为止。
        while (listPos.Count>0)    //数组存放的是需要探索的地方， 数组中存在就会进入探索
        {
            if (searchWay==SearchWay.BFS)
            {
                p = listPos[0];
                listPos.RemoveAt(0);
            }

            if (searchWay==SearchWay.DFS)
            {
                p = listPos[listPos.Count - 1];
                listPos.RemoveAt(listPos.Count - 1);
            }
            //开始 4步走
            if (p.z>0)
            {
                //Debug.Log(1);
                if (Func(p, 0, -1))
                {
                    break;
                }
            }

            if (p.z<H-1)
            {
                //Debug.Log(2);
                if (Func(p,0,1))
                {
                    break;
                }
            }

            if (p.x > 0)
            {
                //Debug.Log(3);
                if (Func(p, -1, 0))
                {
                    break;
                }
            }

            if (p.x < W - 1)
            {
                //Debug.Log(4);
                if (Func(p, 1, 0))
                {
                    break;
                }
            }
            //为什么没有添加进去
            if (step[p.z, p.x] > depth)//遇到一个问题， 会再重复地块上生成路径点并且生成方块。 //因为这里没有给限制每次循环都会去生成了。
            {
                //Debug.Log("数组的内容：" + step[p.z, p.x]);
                depth = step[p.z, p.x];
                RefreshPath();
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield return null;
    }

    bool Func(Pos p, int _x, int _z)     //寻路中
    {
        if (map[p.z + _z, p.x + _x] == ENDCube)  //完成寻路
        {
            //Debug.Log(p.z + "===" + p.x + "====" + endPos.z + "===" + endPos.x);
            step[p.z + _z, p.x + _x] = step[p.z, p.x] + 1;
            gameState = EGameState.ShowPath;
            return true;
        }
        
        if (map[p.z + _z, p.x + _x] == 0)           //当地图上的这个点显示是0时，  也就是避开了 墙  和起点终点。      
        {
            //Debug.Log("huihe"+ step[p.z + _z, p.x + _x] + " "+ step[p.z, p.x]);
            //Debug.Log(string.Format("HH {0} {1} x:{2}_{3} z:{4}_{5}", step[p.z + _z, p.x + _x], step[p.z, p.x], p.x, _x, p.z, _z));
            if (step[p.z + _z, p.x + _x] > step[p.z, p.x] + 1)
            {
                step[p.z + _z, p.x + _x] = step[p.z, p.x] + 1;     //********可能穿墙的原因是因为 这里添加点进去的时候无限制的添加了。     //为什么不是再接近终点的时候停止，而是在终点之前就停止了，
                listPos.Add(new Pos(p.x + _x, p.z + _z));
                //Debug.Log("====a=====" + queue.Count);
                //Debug.Log("==== huihe" + step[p.z + _z, p.x + _x] + " " + step[p.z, p.x]);
                //GameObject g = Instantiate(prefab_path, new Vector3(p.x + _x * 1, 0.5f, p.z + _z * 1), Quaternion.identity);
                //g.transform.Find("number").GetComponent<TextMesh>().text = step[p.z + _z, p.x + _x].ToString();
                if (searchWay == SearchWay.DFS)
                {
                    Pos p1 = new Pos();
                    int n = int.MaxValue;
                    for (int i = 0; i < listPos.Count; i++)
                    {
                        if (Mathf.Abs(listPos[i].x - endPos.x) + Mathf.Abs(listPos[i].z - endPos.z) < n)
                        {
                            n = Mathf.Abs(listPos[i].x - endPos.x) + Mathf.Abs(listPos[i].z - endPos.z);
                            p1 = listPos[i];
                        }
                    }
                    listPos.Add(p1);
                }
            }
        }
        return false;
    }

    void BFSShowPath()     //显示路径
    {
        Pos p = new Pos(endPos.x, endPos.z);
        //试一下， 便利step 步数里面的元素， 然后从1一直++ 到最终找到的步数， 然后直到没有更大步数了， 就停止便利和。 然后加入一个列表，去生成这些步数对应二位数组的坐标的方块来显示路径.
        //从终点开始往回计算， 首先是获得终点的pos。 然后一步步的往回走。
        //首先是上下左右走， 发现是之前来的路，就以这个点为出发点再去计算。 直到最后走到终点为止。
        while (p.x != startPos.x || p.z != startPos.z)
        {
            int n = step[p.z, p.x];
            if (p.z - 1 > 0 && step[p.z - 1, p.x] == n - 1)
            {
                p = new Pos(p.x, p.z - 1);

                Instantiate(prefab_end, new Vector3(p.x * 1, 0.5f, p.z * 1), Quaternion.identity);
            }

            if (p.z + 1 < H - 1 && step[p.z + 1, p.x] == n - 1)
            {
                p = new Pos(p.x, p.z + 1);
                Instantiate(prefab_end, new Vector3(p.x * 1, 0.5f, p.z * 1), Quaternion.identity);
            }

            if (p.x - 1 > 0 && step[p.z, p.x - 1] == n - 1)
            {
                p = new Pos(p.x - 1, p.z);
                Instantiate(prefab_end, new Vector3(p.x * 1, 0.5f, p.z * 1), Quaternion.identity);
            }

            if (p.x + 1 < W - 1 && step[p.z, p.x + 1] == n - 1)
            {
                p = new Pos(p.x + 1, p.z);
                Instantiate(prefab_end, new Vector3(p.x * 1, 0.5f, p.z * 1), Quaternion.identity);
            }
        }
    }

    void RefreshPath()
    {
        GameObject[] all_go = GameObject.FindGameObjectsWithTag("Path");
        foreach (var go in all_go)
        {
            Destroy(go);
        }

        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (map[i, j] == 0 && step[i, j] != int.MaxValue)
                {
                    var go = Instantiate(prefab_path, new Vector3(j * 1, 0.5f, i * 1), Quaternion.identity, prefab_father);
                    go.tag = "Path";
                    go.transform.Find("Number").GetComponent<TextMesh>().text = step[i, j].ToString();
                }
            }
        }
    }

    void DFSShowPath()     //显示路径
    {
        Pos p = new Pos(endPos.x, endPos.z);
        //试一下， 便利step 步数里面的元素， 然后从1一直++ 到最终找到的步数， 然后直到没有更大步数了， 就停止便利和。 然后加入一个列表，去生成这些步数对应二位数组的坐标的方块来显示路径.
        //从终点开始往回计算， 首先是获得终点的pos。 然后一步步的往回走。
        //首先是上下左右走， 发现是之前来的路，就以这个点为出发点再去计算。 直到最后走到终点为止。
        //这个循环是个死循环导致卡死 , 解决了， 给定足够的判定。 
        while (p.x != startPos.x || p.z != startPos.z)
        {
            int n = step[p.z, p.x];

            if (p.z - 1 > 0&&step[p.z - 1, p.x] <= n - 1 && step[p.z - 1, p.x] != int.MaxValue)
            {

                p = new Pos(p.x, p.z - 1);
                Instantiate(prefab_end, new Vector3(p.x * 1, 0.5f, p.z * 1), Quaternion.identity);
            }

            else if (p.z + 1 < H - 1&& step[p.z + 1, p.x] <= n - 1 && step[p.z + 1, p.x] != int.MaxValue)
            {
                p = new Pos(p.x, p.z + 1);
                Instantiate(prefab_end, new Vector3(p.x * 1, 0.5f, p.z * 1), Quaternion.identity);
            }
            else if (p.x - 1 > 0&& step[p.z, p.x - 1] <= n - 1 && step[p.z, p.x - 1] != int.MaxValue)
            {
                p = new Pos(p.x - 1, p.z);
                Instantiate(prefab_end, new Vector3(p.x * 1, 0.5f, p.z * 1), Quaternion.identity);
            }
            else if (p.x + 1 < W - 1&& step[p.z, p.x + 1] <= n - 1 && step[p.z, p.x + 1] != int.MaxValue)
            {
                 p = new Pos(p.x + 1, p.z);
                 Instantiate(prefab_end, new Vector3(p.x * 1, 0.5f, p.z * 1), Quaternion.identity);
            }
            else
            {
                Debug.Log("找不到路了");
                break;
            }
        }


        //for (int i = 0; i < dfslist.Count; i++)             //通过把他们所有的路径父节点添加进一个集合中， 然后再遍历显示这个集合， 就达到了显示路径的效果 但是会有重复的
        //{
        //    Instantiate(prefab_end, new Vector3(dfslist[i].x * 1, 0.5f, dfslist[i].z * 1), Quaternion.identity);
        //}


    }

    #region  astart
    AScore[,] astar_Search;
    IEnumerator AStart()
    {
        astar_Search = new AScore[H, W];
        astar_Search[startPos.z, startPos.x] = new AScore(0, 0);
        listPos.Add(startPos);

        //让list自己调用sort方法去排序，sort会自己调用集合类的元素，两两进行比较， 在比较的时候又拿入到函数去进行比较。
        //这个函数是自己写的， f值越小的放到越前面。 这样就达到了排序的效果 
        while (listPos.Count>0)
        {
            listPos.Sort((Pos p1, Pos p2) =>
            {
                AScore a1 = astar_Search[p1.z, p1.x];
                AScore a2 = astar_Search[p2.z, p2.x];
                //return a1.H.CompareTo(a2.H);
                return a1.CompareTo(a2);
            });

            Pos p = listPos[0];
            listPos.RemoveAt(0);
            astar_Search[p.z, p.x].closed = true;  //进入闭空间     
            if (p.z > 0)
            {
                if (AStartFunc(p, 0, -1)) { break; }
            }
            // 下
            if (p.z < H - 1)
            {
                if (AStartFunc(p, 0, 1)) { break; }
            }
            // 左
            if (p.x > 0)
            {
                if (AStartFunc(p, -1, 0)) { break; }
            }
            // 右
            if (p.x < W - 1)
            {
                if (AStartFunc(p, 1, 0)) { break; }
            }
            
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    if (astar_Search[i,j]!=null)          
                    {
                        step[i, j] = (int)astar_Search[i, j].F;
                    }
                }
            }
            AStartRefreshPath();
            yield return new WaitForSeconds(0.01f);
        }
        gameState = EGameState.ShowPath;
        yield return null;
    }

    bool AStartFunc(Pos _p, int _x, int _z)     //寻路中
    {
        AScore next_aScore = astar_Search[_p.z + _z, _p.x + _x];   //在查找的时候， 先设置一个ascore变量，来等于这个被查找的目标在下一步移动后的pos对应astar_Search二维数组的位置的值。
        if (next_aScore!=null && next_aScore.closed)  //当这个下一步移动后的pos对应ascore在astar_Search数组对应位置已经存在了， 并且这个地方是闭节点。那么直接返回掉不继续对这个路径点进行操作了。
        {
            return false;
        }
        AScore now_aScore = astar_Search[_p.z, _p.x]; //设置一个ascore变量来等于这个被查找的目标的pos对应astar_Search二维数组的位置的值。
        Pos pos = new Pos(_p.x + _x, _p.z + _z);
        if (map[_p.z+_z,_p.x+_x]==ENDCube)  //如果下一步是终点
        {
            AScore a = new AScore(now_aScore.G + 1, 0);  //设置一个新的ascore变量a，代表终点的ascore值， 它的步数等于上一个位置+1， 距离为0，因为已经到了。
            a.parent = _p;
            astar_Search[_p.z + _z, _p.x + _x] = a;  //在astar_search数租中对应的这个位置 的ascore 就是a。
            return true;
        }
        if (map[_p.z+_z,_p.x+_x]==0)  //在地图上面可以行走
        {
            if (next_aScore==null)  //要这一步是空的时候 也就是这里在astar_search数租中还没有赋值。并且是可以走的路
            {
                AScore a = new AScore(now_aScore.G + 1, Pos.AStarDistance(pos, endPos));// 把新找到的路径点 添加成ascore类。 并且放入astar_search数组中，并且保存起来。 用来下次移动时甄别这一个点。是不是已经走过了。
                a.parent = _p;    //为什么这里， 新的路径点的父节点pos 是 上一个路径点的pos。  是为了记录行动的轨迹。
                astar_Search[_p.z + _z, _p.x + _x] = a;
                //
                listPos.Add(pos);
            }
            else if (next_aScore.G>now_aScore.G)   //假如这里已经有点了。 比较双方的步数 如果下面要走的路径点的步数的比当前的大
            {
                next_aScore.G = now_aScore.G + 1; //下一步的步数比现在的增加1
                next_aScore.parent =_p;
                if (!listPos.Contains(pos))
                {
                    listPos.Add(pos);
                }
            }
        }
        return false;
    }

    void AStartRefreshPath() //刷新探索路径
    {
        GameObject[] all_go = GameObject.FindGameObjectsWithTag("Path");
        foreach (var go in all_go)
        {
            Destroy(go);
        }
        for (int i = 0; i < H; i++)
        {
            for (int j = 0; j < W; j++)
            {
                if (map[i, j] == 0 && step[i, j] != int.MaxValue)
                {
                    var go = Instantiate(prefab_path, new Vector3(j * 1, 0.5f, i * 1), Quaternion.identity, prefab_father);
                    go.tag = "Path";
                    go.transform.Find("Number").GetComponent<TextMesh>().text = step[i, j].ToString();
                    PathData pathData = go.GetComponent<PathData>();
                    if (pathData!=null)
                    {
                        pathData.G = astar_Search[i, j].G;
                        pathData.H = astar_Search[i, j].H;
                    }
                }
            }
        }
    }

    void AStartShowPath()     //显示路径
    {
        Pos pos = endPos;
        while(!pos.Equals(startPos))
        {
            GameObject g = Instantiate(prefab_end, new Vector3(pos.x * 1, 0.5f, pos.z * 1), Quaternion.identity, prefab_father);
            pos = astar_Search[pos.z, pos.x].parent;
        }
    }
    #endregion

    //优化后不用了。 
    bool DFSFunc(Pos p, int _x, int _z)     //寻路中           //怎么指导dfs  加上判定， 计算一下 当前点到终点的距离， 如果是
    {
        if (map[p.z + _z, p.x + _x] == ENDCube)  //完成寻路
        {
            //Debug.Log(p.z + "===" + p.x + "====" + endPos.z + "===" + endPos.x + "====" + step[p.z + _z, p.x + _x]);
            step[p.z + _z, p.x + _x] = step[p.z, p.x] + 1;
            gameState = EGameState.ShowPath;
            return true;
        }
        Pos p1 = new Pos();
        if (map[p.z + _z, p.x + _x] == 0)           //当地图上的这个点显示是0时，  也就是避开了 墙  和起点终点。      
        {
            //Debug.Log("huihe"+ step[p.z + _z, p.x + _x] + " "+ step[p.z, p.x]);
            //Debug.Log(string.Format("HH {0} {1} x:{2}_{3} z:{4}_{5}", step[p.z + _z, p.x + _x], step[p.z, p.x], p.x, _x, p.z, _z));
            if (step[p.z + _z, p.x + _x] > step[p.z, p.x] + 1)
            {
                step[p.z + _z, p.x + _x] = step[p.z, p.x] + 1;     //********可能穿墙的原因是因为 这里添加点进去的时候无限制的添加了。     //为什么不是再接近终点的时候停止，而是在终点之前就停止了，
                listPos.Add(new Pos(p.x + _x, p.z + _z));
                int n = int.MaxValue;
                for (int i = 0; i < listPos.Count; i++)
                {
                    if (Mathf.Abs(listPos[i].x - endPos.x) + Mathf.Abs(listPos[i].z - endPos.z) < n)
                    {
                        n = Mathf.Abs(listPos[i].x - endPos.x) + Mathf.Abs(listPos[i].z - endPos.z);
                        p1 = listPos[i];
                    }
                }
                listPos.Add(p1);
                //listPos.RemoveAt(listPos.Count - 2);
                //Debug.Log("====a=====" + queue.Count);
                //Debug.Log("==== huihe" + step[p.z + _z, p.x + _x] + " " + step[p.z, p.x]);
                //GameObject g = Instantiate(prefab_path, new Vector3(p.x + _x * 1, 0.5f, p.z + _z * 1), Quaternion.identity);
                //g.transform.Find("number").GetComponent<TextMesh>().text = step[p.z + _z, p.x + _x].ToString();
            }

        }
        return false;
    }
    IEnumerator DFS()         //开始dfs查找
    {
        step[startPos.z, startPos.x] = 0;
        listPos.Add(startPos);
        Pos p = new Pos();
        //主要的探索循环， 首先拿list中第一个点 来上下左右探寻，  然后将每个新的点都存入到list中， 然后继续探索，直到找到终点为止。
        while (listPos.Count > 0)    //数组存放的是需要探索的地方， 数组中存在就会进入探索
        {

            //dfslist.Add(p);
            //在这里先设置第一个优先级。把当前点的x y 拿出来判断， 如果是在左边， 就先走左， 在上面就上。  下右等同。        
            //遍历数组， 先走离目标最近的
            //开始 4步

            //for (int i = 0; i < listPos.Count; i++) //方案不行。 为什么不行？   因为这个时候的这个点没有经过判定，他可能是在边界， 也可能是死路，然后就会重复的去调用这个所谓的最优的pos， 事实上它哪里也去不了了。
            //{
            //    if (Mathf.Abs(endPos.x + endPos.z)-Mathf.Abs(listPos[i].x + listPos[i].z)  < n)
            //    {
            //        n = (listPos[i].x + listPos[i].z) - (endPos.x + endPos.z);
            //        p = listPos[i];
            //    }
            //}
            //listPos.Remove(p);

            p = listPos[listPos.Count - 1];
            listPos.RemoveAt(listPos.Count - 1);
            if (p.z > 0)
            {
                //Debug.Log(1);
                if (Func(p, 0, -1))
                {
                    break;
                }
            }

            if (p.z < H - 1)
            {
                //Debug.Log(2);
                if (Func(p, 0, 1))
                {
                    break;
                }
            }

            if (p.x > 0)
            {
                //Debug.Log(3);
                if (Func(p, -1, 0))
                {
                    break;
                }
            }

            if (p.x < W - 1)
            {
                //Debug.Log(4);
                if (Func(p, 1, 0))
                {
                    break;
                }
            }

            //为什么没有添加进去
            //if (step[p.z, p.x] > depth) //bfs 需要这个， 但是因为dfs 容易走到死胡同并且存在回溯的问题所以这里加限制会存在一些麻烦。
            RefreshPath();
            //Debug.Log("数组的内容：" + step[p.z, p.x]);
            yield return 0;
        }
        yield return null;
    }
}
