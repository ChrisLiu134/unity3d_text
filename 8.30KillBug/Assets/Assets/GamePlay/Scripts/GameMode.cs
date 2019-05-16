using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {

    //游戏规则击杀30只怪物后出现boss  干掉boss 赢得胜利

    public enum EGameState  //游戏状态枚举   
    {
        /// <summary>
        /// 游戏开始
        /// </summary>
        EGS_Init,
        /// <summary>
        /// 游戏运行中
        /// </summary>
        EGS_Playing,
        /// <summary>
        /// 游戏结束
        /// </summary>
        EGS_End,

    }

    //玩家
    PlayerCharater player;
    UI uI;

    //怪物生成模板 
    GameObject[] enemyNumber;   //怪物集合， 用来算当前怪物数量
    public int enemyNumbers;
    Enemy bugs;
    public Enemy enemys;
    public Enemy Boss;  // 生成boss
    public Transform father;
    int number=100;  // 一次性刷怪的数量
    float Brush_time = 30; //刷怪时间


    GameObject[] planes; // 地板集合

    public EGameState gameState; // 游戏状态机

    public bool game_start; //判断游戏是不是需要开始
    public bool game_in;   //判断进入游戏中
    public bool game_Over;  //判定游戏是否结束
    public bool win_lose;  //判断游戏胜负     
    public bool difficult;
    float time_1 = 0f;  // 计时器

	// Use this for initialization
	void Start () {
        uI = FindObjectOfType<UI>();
        player = FindObjectOfType<PlayerCharater>();
        planes = GameObject.FindGameObjectsWithTag("Plane");
        uI.GameStart();    // 游戏开始界面出现
    }
	
	// Update is called once per frame
	void Update () {
        ChangeState();  // 调整状态
        switch (gameState)  //游戏状态机
        {
            case EGameState.EGS_Init:
                {
                    GameInit();
                }
                break;
            case EGameState.EGS_Playing:
                {
                    GamePlaying();
                }
                break;
            case EGameState.EGS_End:
                {
                    GameOver();
                }
                break;
        }
	}



    void GameInit()  // 初始化游戏  设定开始界面和ui
    { 
        uI.ShowNumebr(player.number);
        uI.ShowGrade(player.grade);
        uI.ShowEnemyNumber(enemyNumbers);

        game_start = false;
        game_Over = false;
        game_in = true;       
    }

    void GamePlaying()  // 游戏中      每一帧计算怪物数量  生成怪物， 判定游戏的胜负，  需要每一帧计算的都在里面
    {       
        enemycount();
        uI.ShowNumebr(player.number);
        uI.ShowEnemyNumber(enemyNumbers);
        uI.ShowGrade(player.grade);
        time_1 += Time.deltaTime;
        if (time_1>= Brush_time)
        {
            if (player.number >= 100)
            {
                SpawnBigBoss();
                uI.ShowBossNow();
                Brush_time = 100000;
            }
            time_1 = 0;
            SpawnEnemy();
            SpawnBoss();
        }
        if (player.grade>=20||player.number>=100)
        {
            game_Over=true;
            win_lose = true;
        }
        if (player.health<=0)
        {
            game_Over = true;
            win_lose = false;
        }
    }

    void GameOver() // 游戏结束  开始清算 点击一个按键游戏结束
    {
        if ( win_lose==true)  //判断游戏胜负     
        {
            uI.ShowWin();
        }
        else
        {
            uI.ShowGameover();
        }       
    }

    void SpawnEnemy() //生成敌人
    {
        //怪物从每一个地板的中心出生， 然后会寻找攻击玩家，
        for (int i = 0; i < planes.Length; i++)
        {            
            Vector3 pos = planes[i].transform.position;
            pos.y += 0.5f;
            Vector3 distance = pos - player.transform.position;

            if (distance.magnitude>20f && number>0) //如果当前刷怪点块离玩家很近则跳过
            {
                bugs=Instantiate(enemys, pos,Quaternion.identity, father);
                if (difficult==true)
                {
                    bugs.moveSpeed += 0.15f;
                    bugs.transform.localScale = bugs.transform.localScale * 2;//这里用bugs 可以接住一个该脚本物体的返回值， 然后我可以对返回值进行任意修改， 修改的就是该物体的属性，而用会修改原物体的属性。
                }               
                number--;
            }
        }
    }

    void SpawnBoss()  //生成小boss
    {
        if (player.grade >= 3&&number>=0)
        {
            Vector3 bossPos = new Vector3(0, 100, 0);
            bugs=Instantiate(Boss, bossPos, Quaternion.identity, father);
            if (difficult == true)
            {
                bugs.moveSpeed += 0.1f;
                bugs.transform.localScale = bugs.transform.localScale * 2;//这里用bugs 可以接住一个该脚本物体的返回值， 然后我可以对返回值进行任意修改， 修改的就是该物体的属性，而用会修改原物体的属性。
            }
        }
    }

    void SpawnBigBoss()  //生成超级大boss
    {
        Vector3 bossPos = new Vector3(0, 100, 0);
        bugs=Instantiate(Boss, bossPos, Quaternion.identity, father);
        bugs.transform.localScale = bugs.transform.localScale * 2;
        if (difficult == true)
        {
            bugs.moveSpeed += 0.05f;
            bugs.transform.localScale = bugs.transform.localScale * 2;//这里用bugs 可以接住一个该脚本物体的返回值， 然后我可以对返回值进行任意修改， 修改的就是该物体的属性，而用会修改原物体的属性。
        }
    }

    public void ChangeState()  // 切换状态机状态
    {
        if (game_start==true)
        {
            gameState = EGameState.EGS_Init;
        }

        if (game_in==true)
        {
            gameState = EGameState.EGS_Playing;
        }

        if (game_Over==true)
        {
            gameState = EGameState.EGS_End;
        }
    }

    public void enemycount()  //数怪物数量
    {
        enemyNumbers = 0;           //把数量归零然后重新计算
        enemyNumber = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyNumber!=null)
        {
            for (int i = 0; i < enemyNumber.Length; i++)
            {
                enemyNumbers++;
            }
        }
    }

    public void Simple()  // 简单难度
    {
         number = 100;  // 小怪总数量
         Brush_time = 30; //刷怪时间
    }

    public void Medium() //中等难度
    {
        number = 300;
        Brush_time = 20;
    }

    public void Difficult()  // 恶心难度
    {
        number = 500;
        Brush_time = 12;
        difficult = true;
       //在这里给怪物的属性做了修改之后会一直产生影响， 如果使用的是private则无法修改，需要在结束游戏的时候重置，或者在游戏开始的时候就设置好一个初始速度。在游戏结束的时候去重置他。
    }

}
