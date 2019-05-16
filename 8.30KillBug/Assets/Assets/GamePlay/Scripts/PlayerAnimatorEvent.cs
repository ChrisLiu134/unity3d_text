using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvent : MonoBehaviour {

    PlayerCharater player;
    //先选择好动画的碰撞体
    public Collider leftSword;
    public Collider rightSword;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerCharater>();

        //先把碰撞体都关掉
        leftSword.enabled= false;
        rightSword.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnAnimationEvent_LeftAttackStart() // 左手攻击动画开始时帧事件
    {
        leftSword.enabled = true;
        rightSword.enabled = false;
    }

    public void OnAnimationEvent_LeftAttackEnd()// 左手攻击动画结束时帧事件
    {
        leftSword.enabled = false;
    }

    public void OnAnimationEvent_RightAttackStart() // 右手开始
    {
        leftSword.enabled = false;
        rightSword.enabled = true;
    }

    public void OnAnimationEvent_RightAttackEnd()  // 右手结束
    {
        rightSword.enabled = false;
    }
}
