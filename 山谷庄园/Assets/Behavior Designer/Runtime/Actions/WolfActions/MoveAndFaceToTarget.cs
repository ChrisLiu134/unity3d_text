using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


[TaskCategory("WolfActions")]
[TaskDescription("靠近和朝向目标")]
public class MoveAndFaceToTarget : Action {
    WolfCha wolfCha;

    public override void OnStart()
    {
        wolfCha = GetComponent<WolfCha>();
    }


    public override TaskStatus OnUpdate()
    {
        wolfCha.MoveAndFaceToTarget();
        return TaskStatus.Running;
    }


}
