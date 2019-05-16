using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameMode : MonoBehaviour {

    /// <summary>
    /// 游戏状态机
    /// </summary>
    public enum EGameState
    {
        Start,
        playing,
        end,     
    }
    public EGameState gameState;

    UIManager uIManager;
    // Use this for initialization

    private void Awake()
    {
        uIManager = FindObjectOfType<UIManager>();

    }
    void Start () {
        gameState = EGameState.Start;
	}
	
	// Update is called once per frame
	void Update () {
        switch(gameState)
        {
            case EGameState.Start:
                GameStart();
                break;
            case EGameState.playing:
                GamePlaying();
                break;
            case EGameState.end:
                GameEnd();
                break;
        }
	}

    /// <summary>
    /// 切换游戏状态,给其它地方调用
    /// </summary>
    public void ChangeState(GameMode.EGameState eGameState)
    {
        gameState = eGameState;
    }

    /// <summary>
    /// 游戏开始时
    /// </summary>
    void GameStart()
    {
        uIManager.OpenWindow(uIManager.startWindow);
        uIManager.CloseWindow(uIManager.shopWindow);
        GameObject obj = GameObject.Find("GreenCircle").transform.parent.gameObject;
        uIManager.currenObj = obj;
    }

    /// <summary>
    /// 游戏中
    /// </summary>
    void GamePlaying()
    {
        uIManager.CloseWindow(uIManager.startWindow);
        SkillManager.Instance().CountCd();
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    void GameEnd()
    {

    }

}
