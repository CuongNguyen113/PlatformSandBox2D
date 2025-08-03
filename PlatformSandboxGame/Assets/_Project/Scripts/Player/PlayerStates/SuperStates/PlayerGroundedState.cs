 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGroundedState : PlayerState {

    private PlayerInputHandler playerInputHandler; 

    protected int movementXInput;
    protected int movementYInput;

    protected bool isTouchingCeiling;

    private bool isGrounded;
    private bool jumpInput;
    private bool grabInput;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool dashInput;

    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {

    }

    public override void DoCheck() {
        base.DoCheck();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingLedge = player.CheckIfTouchingLedge();
        isTouchingCeiling = player.CheckForCeiling();
    }

    public override void Enter() {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
        player.DashState.ResetCanDash();

        playerInputHandler = player.InputHandler;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        movementXInput = playerInputHandler.NormInputX;
        movementYInput = playerInputHandler.NormInputY;
        jumpInput = playerInputHandler.JumpInput;
        grabInput = playerInputHandler.GrabInput;
        dashInput = playerInputHandler.DashInput;

        if (playerInputHandler.AttackInput[(int)CombatInput.primary] && !isTouchingCeiling) {
            playerStateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (playerInputHandler.AttackInput[(int)CombatInput.secondary] && !isTouchingCeiling) {
            playerStateMachine.ChangeState(player.SecondaryAttackState);
        }
        if (jumpInput && player.JumpState.CanJump() && !isTouchingCeiling) {
            playerStateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded) {
            player.InAirState.StartCoyoteTime();
            playerStateMachine.ChangeState(player.InAirState);
        }
        else if (isTouchingWall && grabInput && isTouchingLedge) {
            playerStateMachine.ChangeState(player.WallGrabState);
        } 
        else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling) {
            playerStateMachine.ChangeState(player.DashState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
