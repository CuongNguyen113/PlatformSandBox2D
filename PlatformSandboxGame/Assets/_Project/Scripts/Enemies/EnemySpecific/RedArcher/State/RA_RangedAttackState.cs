using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RA_RangedAttackState : RangedAttackState {

    private RedArcher redArcher;

    public RA_RangedAttackState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, Transform attackPosition, RangedAttackSO rangedAttackSO, RedArcher redArcher) : base(entity, finiteStateMachine, animBoolName, attackPosition, rangedAttackSO) {
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

        if (isAnimationFinished) {
            if (isPlayerInMinAgroRange) {
                _finiteStateMachine.ChangeState(redArcher.playerDetectedState);
            }
            else {
                _finiteStateMachine.ChangeState(redArcher.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
