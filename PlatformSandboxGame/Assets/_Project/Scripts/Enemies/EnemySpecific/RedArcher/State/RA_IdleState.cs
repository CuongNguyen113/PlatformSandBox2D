using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RA_IdleState : IdleState {

    private RedArcher redArcher;
    public RA_IdleState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, IdleSO idleSO, RedArcher redArcher) : base(entity, finiteStateMachine, animBoolName, idleSO) {
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

        if (isPlayerInMinAgroRange) {
            _finiteStateMachine.ChangeState(redArcher.playerDetectedState);
        }
        else if (isIdleTimerOver) {
            _finiteStateMachine.ChangeState(redArcher.moveState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
