    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("WolfConditionals")]
[TaskDescription("判断是不是丢失战斗目标了")]
public class LoseAttackTarget : Conditional {
    WolfCha wolfCha;
    public override void OnStart()
    {
        wolfCha = GetComponent<WolfCha>();
    }

    public override TaskStatus OnUpdate()
    {
        if (wolfCha.LoseTarget())
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }


}
