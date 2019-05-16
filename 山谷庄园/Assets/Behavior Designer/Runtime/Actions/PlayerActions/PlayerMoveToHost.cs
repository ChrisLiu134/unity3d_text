using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerActions")]
[TaskDescription("随从跟着玩家")]
public class PlayerMoveToHost : Action {
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        playerCha.MoveToHost();
        return TaskStatus.Success;
    }

}
