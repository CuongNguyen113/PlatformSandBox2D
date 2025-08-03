using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState {

    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {

    }

    public override void DoCheck() {
        base.DoCheck();
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        player.CheckIfShouldFlip(movementXInput);
        player.SetVelocityX(playerData.movementVelocity * movementXInput);

        if (!isExitingState) {
            if (movementXInput == 0) {
                playerStateMachine.ChangeState(player.IdleState);
            }
            else if (movementYInput == -1) {
                playerStateMachine.ChangeState(player.CrouchMoveState);
            }
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
