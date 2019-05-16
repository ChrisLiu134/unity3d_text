using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


[TaskCategory("WolfActions")]
[TaskDescription("在巡逻状态中，巡逻的行为")]
public class OnPatrol : Action {
    WolfCha wolfCha;

    public override void OnStart()
    {
        wolfCha = GetComponent<WolfCha>();
    }


    public override TaskStatus OnUpdate()
    {

        wolfCha.Patrol();
        
        return TaskStatus.Success;
    }

}
