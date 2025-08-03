using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RA_DodgeState : DodgeState {

    private RedArcher redArcher;
    public RA_DodgeState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, DodgeSO dodgeSO, RedArcher redArcher) : base(entity, finiteStateMachine, animBoolName, dodgeSO) {
        this.redArcher = redArcher;
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (isDodgeOver) {
            if (isPlayerInMaxAgroRange && performCloseRangeAction) {
                _finiteStateMachine.ChangeState(redArcher.meleeAttackState);
            } 
            else if (isPlayerInMaxAgroRange && !performCloseRangeAction && !isDetectingWallBehind) {
                _finiteStateMachine.ChangeState(redArcher.rangedAttackState);
            } 
            else if (isDetectingWallBehind) {
                _finiteStateMachine.ChangeState(redArcher.moveState);
            } 
            else if (!isPlayerInMaxAgroRange) {
                _finiteStateMachine.ChangeState(redArcher.lookForPlayerState);
            }

        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

    }
}
