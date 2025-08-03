using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState {

    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool grabInput;
    protected bool jumpInput;
    protected bool isTouchingLedge;
    protected int movementInputX;
    protected int movementInputY; 

    public PlayerTouchingWallState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {
    }

    public override void AnimationFinishTrigger() {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger() {
        base.AnimationTrigger();
    }

    public override void DoCheck() {
        base.DoCheck();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingLedge = player.CheckIfTouchingLedge();

        if (isTouchingWall && !isTouchingLedge) {
            player.LedgeClimbState.SetDetectedPos(player.transform.position);
        }
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        movementInputX = player.InputHandler.NormInputX;
        movementInputY = player.InputHandler.NormInputY;
        grabInput = player.InputHandler.GrabInput;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput) {
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            playerStateMachine.ChangeState(player.WallJumpState);
        }
        else if (isTouchingWall && !isTouchingLedge) {
            playerStateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (isGrounded && !grabInput) {
            playerStateMachine.ChangeState(player.IdleState);
        }
        else if (!isTouchingWall || (movementInputX != player.FacingDirection && !grabInput)) {
            playerStateMachine.ChangeState(player.InAirState);
        }
    }



    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
