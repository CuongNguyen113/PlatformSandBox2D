using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerWallClimbState : PlayerTouchingWallState {
    public PlayerWallClimbState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (!isExitingState) {
            player.SetVelocityY(playerData.wallClimbVelocity);

            if (movementInputY != 1) {
            
                playerStateMachine.ChangeState(player.WallGrabState);
            }
        }

    }
}
