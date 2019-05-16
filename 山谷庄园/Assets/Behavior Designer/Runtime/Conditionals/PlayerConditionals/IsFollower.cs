﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerConditionals")]
[TaskDescription("判断当前玩家角色是不是跟随者")]
public class IsFollower : Conditional {
    PlayerCharater playerCha;
    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        if (playerCha.IsFollower())
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
