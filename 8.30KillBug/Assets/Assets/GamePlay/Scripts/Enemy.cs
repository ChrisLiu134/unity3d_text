using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //怪物寻找玩家并碰撞来攻击
    Rigidbody rigidbody;
    public float moveSpeed;
    public int health;
    public int attack;
    public bool isAlive;
    public bool attackState;
    PlayerCharater player;
    PlayerController controller;
    Vector3 tragetPos;
    Animator animator;
    GameMode gameMode;

    float time_1 = 0; // 计时器

    // Use this for initialization
    void Start () {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerCharater>();
        gameMode = FindObjectOfType<GameMode>();
        controller = FindObjectOfType<PlayerController>();
        isAlive = true;
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameMode.game_Over==true)
        {
            return;
        }
        animator.SetBool("IsAlive", isAlive);
        if (!isAlive)
        {
            return;
        }
        LookFor();
        AttackState(); //攻击状态内
    }

    public void OnHit(int attack)
    {
        animator.SetTrigger("OnHit");
        health -= attack;
    }

    public void Die()
    {
        if (!isAlive)
        {
            return;
        }        
        isAlive = false;

        player.number += 1;        //怪物死亡后加经验和杀怪数量
        if (transform.name=="EnemyBoss")
        {
            
            player.EXP += 40;
        }
        else
        {
            player.EXP += 20;
        }
        controller.RefreshEXP((float)player.EXP / 100f);
        Destroy(gameObject, 5);
    }

    void Attack()  //攻击
    {
        if (isAlive==false)
        {
            return;
        }
        if (attackState==true)
        {
            animator.SetTrigger("EnemyAttack");
            player.OnHit(attack);
            if (player.health <= 0)
            {
                player.Die();
            }
        }
       
    }

    //攻击系统， 第一次碰到直接攻击
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject==player.gameObject)
        {
            attackState = true;
            Attack();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        attackState = false;
    }

    void AttackState()  //攻击中状态
    {
        if (attackState==true)
        {
            time_1  += Time.deltaTime;
            if (time_1>=2f)
            {
                Attack();
                time_1 = 0;
            }
        }
    }

    void LookFor()  //寻找目标
    {
        tragetPos = player.transform.position - transform.position;
        rigidbody.velocity = tragetPos * moveSpeed;
        transform.LookAt(player.transform);
    }

}
