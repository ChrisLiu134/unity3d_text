﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerActions")]
[TaskDescription("玩家死亡后重生")]
public class PlayerRevive : Action {
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        playerCha.Revive();
        return TaskStatus.Success;
    }
}
