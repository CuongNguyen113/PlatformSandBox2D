using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState {
    public PlayerCrouchIdleState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {
    }

    public override void Enter() {
        base.Enter();

        player.SetVelocityX(0f);
        player.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void Exit() {
        base.Exit();

        player.SetColliderHeight(playerData.standColliderHeight);
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (!isExitingState) {
            if (movementXInput != 0) {
                playerStateMachine.ChangeState(player.CrouchMoveState);
            }
            else if (movementYInput != -1 && !isTouchingCeiling) {
                playerStateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
