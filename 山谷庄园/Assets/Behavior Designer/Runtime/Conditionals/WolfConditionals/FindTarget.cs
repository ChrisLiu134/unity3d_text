using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("WolfConditionals")]
[TaskDescription("判断是不是发现战斗目标了")]
public class FindTarget : Conditional {

    WolfCha wolfCha;
    public override void OnStart()
    {
        wolfCha = GetComponent<WolfCha>();
    }

    public override TaskStatus OnUpdate()
    {
        if (wolfCha.FindAttackTarget()||wolfCha.currenAttacktarget!=null)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
