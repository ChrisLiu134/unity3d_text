using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("WolfConditionals")]
[TaskDescription("判断自己是不是已经死亡了")]
public class IsDead : Conditional {
    WolfCha wolfCha;
    public override void OnStart()
    {
        wolfCha = GetComponent<WolfCha>();
       
    }

    public override TaskStatus OnUpdate()
    {
        if (wolfCha.wolf.hp<=0)
        {
            wolfCha.dead();
            wolfCha.wolf.hp = 0;
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
