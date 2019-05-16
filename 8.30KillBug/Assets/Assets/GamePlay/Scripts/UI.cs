using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public Text numberText;
    public Text GradeText;
    public Text nameText;
    public Text EnemyNumber;
    public GameObject bossNow;

    public GameObject gamestart;
    public GameObject gameWin;
    public GameObject gameOver;

    public GameObject choice1;
    public GameObject choice2;
    public GameObject choice3;

    public Slider HPSlider;
    public Slider EXPSlider;

    GameMode gameMode;

	// Use this for initialization
	void Start () {
        gameMode = FindObjectOfType<GameMode>();
    }
	
	// Update is called once per frame
	void Update () {
    }


    public void ShowNumebr(int number)   //显示分数
    {
        numberText.text = "数量： " + number.ToString();
    }

    public void ShowGrade(int grade) //显示等级
    {
        GradeText.text = "等级: " + grade.ToString();
    }
    public void ShowEnemyNumber(int number)
    {
        EnemyNumber.text = "你还需要宰杀" + number.ToString() + "只虫子";
    }

    public void ShowHP(float hp)  // 显示生命值
    {
        HPSlider.value = hp;
    }

    public void ShowEXP(float exp) //显示经验值
    {
        EXPSlider.value = exp;
    }

    public void ShowWin()  //显示游戏胜利的ui
    {
        gameWin.SetActive(true);
        gameOver.SetActive(false);
    }

    public void ShowGameover()
    {
        gameWin.SetActive(false);
        gameOver.SetActive(true);

    }

    public void ShowBossNow()
    {
        bossNow.SetActive(true);
    }

    public void Restart()   // 重开按键
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("KillBug");
    }

    public void Exit() // 退出按键
    {
        Application.Quit();
    }

    public void GameStart()  //游戏开始 显示开头画面
    {
        gamestart.SetActive(true);
    }

    public void StartButton()  //游戏开始的按键
    {
        gameMode.game_start = true;
        gamestart.SetActive(false);
    }

    public void Choice1()
    {
        gameMode.Simple();
    }

    public void Choice2()
    {
        gameMode.Medium();
    }

    public void Choice3()
    {
        gameMode.Difficult();
    }

}
