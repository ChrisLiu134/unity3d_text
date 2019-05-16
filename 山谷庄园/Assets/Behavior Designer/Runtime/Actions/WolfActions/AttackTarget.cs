using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


[TaskCategory("WolfActions")]
[TaskDescription("攻击的动作")]
public class AttackTarget : Action {
    WolfCha wolfCha;
    GameObject target;
    public override void OnStart()
    {
        wolfCha = GetComponent<WolfCha>();
        target = wolfCha.currenAttacktarget;
    }

    public override TaskStatus OnUpdate()
    {
        wolfCha.MonsterAttack(target.GetComponent<PlayerCharater>());
        return TaskStatus.Success;
    }

}
