using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerActions")]
[TaskDescription("走回原点")]
public class PlayerBackToScool :Action  {
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        if (playerCha.fllowTarget.GetComponent<PlayerCharater>().player.isAlive==true)
        {
            return TaskStatus.Success;
        }
        playerCha.Moving(playerCha.playerRevivePoint.transform.position);
        return TaskStatus.Running;
    }

}
