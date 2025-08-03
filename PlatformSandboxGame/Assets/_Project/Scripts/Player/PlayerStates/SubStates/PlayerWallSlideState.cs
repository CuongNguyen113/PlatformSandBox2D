using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState {

    public PlayerWallSlideState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (!isExitingState) {
            player.SetVelocityY(-playerData.wallSlideVelocity);

            if (grabInput && movementInputY == 0) {
                playerStateMachine.ChangeState(player.WallGrabState);
            }
        }
    }
}
