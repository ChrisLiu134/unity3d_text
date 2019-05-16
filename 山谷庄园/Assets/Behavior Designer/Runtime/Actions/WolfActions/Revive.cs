using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("WolfActions")]
[TaskDescription("复生在原点")]
public class Revive : Action{
    WolfCha wolfCha;

    public override void OnStart()
    {
        wolfCha = GetComponent<WolfCha>();
    }

    public override TaskStatus OnUpdate()
    {
        wolfCha.Relive();
        return TaskStatus.Success;
    }

}
