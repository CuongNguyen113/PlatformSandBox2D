using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;

    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;
    private bool isTouchingCeiling;

    private int movementInputX;
    private int movementInputY;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {
    }

    public override void AnimationTrigger() {
        base.AnimationTrigger();

        player.Animator.SetBool(PlayerStateConstants.LEDGE_CLIMB, false);

        isHanging = true;
    }

    public override void Enter() {
        base.Enter();

        player.SetVelocityZero();
        player.transform.position = detectedPos;
        cornerPos = player.DetermineCornerPosition();

        startPos.Set(cornerPos.x - (player.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (player.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;
    }

    public override void Exit() {
        base.Exit();

        isHanging = false;
        if (isClimbing) {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (isAnimationFinish) {
            if (isTouchingCeiling) {
                playerStateMachine.ChangeState(player.CrouchIdleState);
            } else {
                playerStateMachine.ChangeState(player.IdleState);
            }
        } 
        else {
            movementInputX = player.InputHandler.NormInputX;
            movementInputY = player.InputHandler.NormInputY;

            jumpInput = player.InputHandler.JumpInput;

            player.SetVelocityZero();
            player.transform.position = startPos;

            if (movementInputX == player.FacingDirection && isHanging && !isClimbing) {
                CheckForSpace();
                isClimbing = true;
                player.Animator.SetBool(PlayerStateConstants.LEDGE_CLIMB, true);
            }
            else if (movementInputY == -1 && !isClimbing) {
                playerStateMachine.ChangeState(player.InAirState);
            }
        }

    }

    public void SetDetectedPos(Vector2 pos) => detectedPos = pos;

    private void CheckForSpace() {
        isTouchingCeiling = Physics2D.Raycast(cornerPos + (Vector2.up * Player.tolerancesLedgeCLimb) + (Vector2.right * player.FacingDirection * Player.tolerancesLedgeCLimb)
                                            , Vector2.up, playerData.standColliderHeight, playerData.whatIsGround);

        player.Animator.SetBool(PlayerStateConstants.IS_TOUCHING_CEILING, isTouchingCeiling);
    }
}
