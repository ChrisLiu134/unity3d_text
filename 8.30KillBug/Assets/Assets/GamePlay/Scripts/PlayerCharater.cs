using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharater : MonoBehaviour {

    Rigidbody rigidbody;
    PlayerController controller;
    public float moveSpeed;
    public int health;
    public bool isAlive;

    public int maxHP=200;
    public int EXP = 0;   // 经验值
    public int grade = 0; //等级
    public int number=0; // 杀怪数量
    int a=1;  // 设置攻击动作的变换
    //先设置两个需要变换的材质
    public Material materialBlack;
    public Material materialWhite;
    

    public ParticleSystem attackLeftFX; // 左攻击特效
    public ParticleSystem attackRightFX;// 右攻击特效

    Animator animator;
                                        
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        controller = FindObjectOfType<PlayerController>();
        isAlive = true;
        attackLeftFX.Stop();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isAlive)
        {
            return;
        }
        animator.SetBool("IsAlive", isAlive);
        RefreshMaxHP();
        UpGrade();
    }

    public void Move(Vector3 move)
    {
        rigidbody.velocity = move * moveSpeed;
        
    }

    public void Attack()   // 先只弄一个左手攻击
    {
        
        if (a==1)
        {
            animator.SetTrigger("Attack_l");
            attackLeftFX.Play();
            a += 1;
        }
        else if(a==2)
        {
            animator.SetTrigger("Attak_r");
            attackRightFX.Play();
            a = 1;
        }
       
    }

    public void OnHit(int attack) // 被打
    {
        health -= attack;
        controller.RefreshHP((float)health / (float)maxHP); 
        animator.SetTrigger("Faint");
    }


    public void RefreshMaxHP()  //血量上限检测， 避免超出
    {
        if (health > maxHP)
        {
            maxHP = health;
        }
        if (health<=100)
        {
            maxHP = 100;
        }
    }
    public void Die()
    {
        if (!isAlive)
        {
            return;
        }
        isAlive = false;
    }

    void UpGrade()  //升级了
    {
        if (EXP>=100)
        {
            grade += 1;
            health += 100;
            EXP -= 100;
        }
        controller.RefreshEXP((float)EXP / 100f);
    }

    //如果碰到了东西的父类是planes 那么就把这个地板设为中心板子  
    //当我持续碰到这块地板时， 他的标签是centre  但是我离开后 就变为边界板子。实现了。
    //if (collision.gameObject.transform.parent.name == "Planes")    这段话要先判断 parent是不是null  需要考虑到这个情况。
    //{
    //    collision.gameObject.tag = "PeriPhery";
    //}

    private void OnCollisionStay(Collision collision)  
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            collision.gameObject.GetComponent<MeshRenderer>().material = materialWhite;   //变换材质的方法
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            collision.gameObject.GetComponent<MeshRenderer>().material = materialBlack;
        }
    }
}
