using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerConditionals")]
[TaskDescription("玩家移动中判断是不是点击到了敌人")]
public class PlayerFindTarget : Conditional {
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();

    }

    public override TaskStatus OnUpdate()
    {
        if (playerCha.FindTarget())
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
