using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInAirState : PlayerState {

    //Input 
    private PlayerInputHandler playerInputHandler;
    private int movementXInput;
    private bool jumpInput;
    private bool grabInput;
    private bool jumpInputStop;
    private bool dashInput; 
    private bool IsGrounded;

    //Checks
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isTouchingLedge;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;


    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool isJumping;

    private float startWallJumpCoyoteTime;
    public PlayerInAirState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {
    }

    public override void DoCheck() {
        base.DoCheck();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        IsGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();
        isTouchingLedge = player.CheckIfTouchingLedge();

        if (isTouchingWall && !isTouchingLedge) {
            player.LedgeClimbState.SetDetectedPos(player.transform.position);
        }

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack)) {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter() {
        base.Enter();

        playerInputHandler = player.InputHandler;
    }

    public override void Exit() {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        movementXInput = playerInputHandler.NormInputX;
        jumpInput = playerInputHandler.JumpInput;
        jumpInputStop = playerInputHandler.JumpInputStop;
        grabInput = playerInputHandler.GrabInput;
        dashInput = playerInputHandler.DashInput;

        CheckJumpMultiplier();

        if (playerInputHandler.AttackInput[(int)CombatInput.primary]) {
            playerStateMachine.ChangeState(player.PrimaryAttackState);
        } else if (playerInputHandler.AttackInput[(int)CombatInput.secondary]) {
            playerStateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (IsGrounded && player.CurrentVelocity.y <= 0.01f) {
            playerStateMachine.ChangeState(player.LandState);
        } else if (isTouchingWall && !isTouchingLedge && !IsGrounded) {
            playerStateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime)) {
            StopWallJumpCoyoteTime();
            isTouchingWall = player.CheckIfTouchingWall();
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            playerStateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump()) {
            playerStateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && grabInput && isTouchingLedge) {
            playerStateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && movementXInput == player.FacingDirection && player.CurrentVelocity.y <= 0f) {
            playerStateMachine.ChangeState(player.WallSlideState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash()) {
            playerStateMachine.ChangeState(player.DashState);
        }
        else {
            player.CheckIfShouldFlip(movementXInput);
            player.SetVelocityX(playerData.movementVelocity * movementXInput);

            player.Animator.SetFloat(PlayerAnimationConstants.Y_VELOCITY, player.CurrentVelocity.y);
            player.Animator.SetFloat(PlayerAnimationConstants.X_VELOCITY, Mathf.Abs(player.CurrentVelocity.x));
        }
    }


    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    private void CheckCoyoteTime() {
        if (coyoteTime && Time.time > startStateTime + playerData.coyoteTime) {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime() {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime) {
            wallJumpCoyoteTime = false;
        }
    }
    
    private void CheckJumpMultiplier() {
        if (isJumping) {
            if (jumpInputStop) {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMulptiplier);
                isJumping = false;
            } else if (player.CurrentVelocity.y <= 0) {
                isJumping = false;
            }
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
    public void StartWallJumpCoyoteTime() {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;
}
