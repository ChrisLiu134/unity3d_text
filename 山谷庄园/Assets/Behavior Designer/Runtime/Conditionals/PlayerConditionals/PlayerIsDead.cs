using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerConditionals")]
[TaskDescription("判断当前玩家角色是不是死亡了")]
public class PlayerIsDead : Conditional {
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        if (playerCha.player.hp<=0)
        {
            playerCha.dead();
            playerCha.player.hp = 0;
            return TaskStatus.Failure;
        }
        return TaskStatus.Running;
    }
}
