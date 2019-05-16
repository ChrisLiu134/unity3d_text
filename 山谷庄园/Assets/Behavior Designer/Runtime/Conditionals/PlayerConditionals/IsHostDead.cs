using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerConditionals")]
[TaskDescription("角色跟随中判断主机是不是挂了")]
public class IsHostDead : Conditional{

    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();

    }

    public override TaskStatus OnUpdate()
    {
        if (playerCha.fllowTarget.GetComponent<PlayerCharater>().player.isAlive==false)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
