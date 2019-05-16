using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerConditionals")]
[TaskDescription("判断当前玩家是不是已经接近目标了")]
public class PlayerFarForHost : Conditional
{
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        if ((transform.position-playerCha.fllowTarget.transform.position).magnitude>3f
            &&playerCha.currenBattleTraget==null
            && playerCha.fllowTarget.GetComponent<PlayerCharater>().player.isAlive == true) //当前自己离主机太远，并且自己没有战斗目标 //并且当前主机角色存活
        {
            return TaskStatus.Success;
        }

        if (playerCha.fllowTarget.GetComponent<PlayerCharater>().currenBattleTraget!=null)
        {
            playerCha.currenBattleTraget = playerCha.fllowTarget.GetComponent<PlayerCharater>().currenBattleTraget;
        }
        return TaskStatus.Running;
    }
}
