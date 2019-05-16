using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerConditionals")]
[TaskDescription("判断当前玩家角色是不是离开战斗了")]
public class PlayerLeaveBattle : Conditional {
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        if (playerCha.PlayerLoseTarget())  //当玩家主动走开，或者当前战斗目标死亡了,或者自己跟随的对象 没有战斗了
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

}
