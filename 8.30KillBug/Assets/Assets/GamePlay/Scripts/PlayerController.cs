using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    PlayerCharater player;
    GameMode gameMode;
    UI ui;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerCharater>();
        gameMode = FindObjectOfType<GameMode>();
        ui = FindObjectOfType<UI>();

        
    }
	
	// Update is called once per frame
	void Update () {
        if (!player.isAlive||gameMode.game_Over==true)     
        {
            return;
        }
        ConMove();
        if (Input.GetMouseButtonDown(0))
        {
            player.Attack();
        }
    }

    /// <summary>
    /// 控制移动
    /// </summary>
    void ConMove()
    {
        float ad = Input.GetAxis("Horizontal");
        float ws = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(ad, 0, ws);     
        player.Move(move);
    }

    public void RefreshHP(float tatio)  //刷新生命值
    {
        ui.ShowHP(tatio);
    }
    public void RefreshEXP(float tatio) //刷新经验值
    {
        ui.ShowEXP(tatio);
    }


}
