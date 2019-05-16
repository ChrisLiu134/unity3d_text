using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("WolfConditionals")]
[TaskDescription("判断是不是接近目标了，是的就返回正确执行下一个行为")]
public class CloseToTarget : Conditional {
    WolfCha wolfCha;

    public float AttackDistance;
    public override void OnStart()
    {
        wolfCha = GetComponent<WolfCha>();
        AttackDistance = 2f;
    }

    public override TaskStatus OnUpdate()
    {
        if ((wolfCha.currenAttacktarget.transform.position - transform.position).magnitude < AttackDistance)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
