using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("WolfActions")]
[TaskDescription("返回起始点，然后开始巡逻")]
public class Back : Action {
    WolfCha wolfCha;
    public override void OnStart()
    {
        wolfCha = GetComponent<WolfCha>();
    }

    public override TaskStatus OnUpdate()
    {
        wolfCha.currenAttacktarget = null;
        if (wolfCha.Back())
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

}
