using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState {

    private int wallJumpDirection;
    public PlayerWallJumpState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {
    }

    public override void Enter() {
        base.Enter();

        player.InputHandler.UseJumpInput();
        player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        player.CheckIfShouldFlip(wallJumpDirection);
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        player.Animator.SetFloat(PlayerAnimationConstants.Y_VELOCITY, player.CurrentVelocity.y);
        player.Animator.SetFloat(PlayerAnimationConstants.X_VELOCITY, Mathf.Abs(player.CurrentVelocity.x));

        if (Time.time >= startStateTime + playerData.wallJumpTime) {
            isAbilityDone = true;
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall) {
        if (isTouchingWall) {
            wallJumpDirection = -player.FacingDirection;
        } 
        else {
            wallJumpDirection = player.FacingDirection;
        }
    }
}
