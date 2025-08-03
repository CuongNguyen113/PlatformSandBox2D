using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState {
    public PlayerLandState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        if (!isExitingState) {
            if (movementXInput != 0) {
                playerStateMachine.ChangeState(player.MoveState);
            } else if (isAnimationFinish) {
                playerStateMachine.ChangeState(player.IdleState);
            }
        }
        
    }
}
