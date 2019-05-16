using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("PlayerActions")]
[TaskDescription("玩家对当前敌人发动攻击")]
public class PlayerAttack : Action {
    PlayerCharater playerCha;

    public override void OnStart()
    {
        playerCha = GetComponent<PlayerCharater>();
    }

    public override TaskStatus OnUpdate()
    {
        if (playerCha.currenBattleTraget!=null)
        {
            playerCha.PlayerAttack(playerCha.currenBattleTraget.GetComponent<WolfCha>());
        }
        return TaskStatus.Running;
    }
    
}
