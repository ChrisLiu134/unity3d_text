using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerConditionals")]
[TaskDescription("判断当前玩家是不是已经接近目标了")]
public class PlayerCloseTarget : Conditional{
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        if (playerCha.CloseToTarget())
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
