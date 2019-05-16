using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerActions")]
[TaskDescription("玩家移动和朝向鼠标点击的敌人目标")]
public class PlayerMoveAndFaceTarget : Action {
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        playerCha.CombatDistance();
        return TaskStatus.Running;
    }
}
