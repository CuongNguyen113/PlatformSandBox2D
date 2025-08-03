using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RA_LookForPlayerState : LookForPlayerState {

    private RedArcher redArcher;
    public RA_LookForPlayerState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, LookForPlayerSO lookForPlayerSO, RedArcher redArcher) : base(entity, finiteStateMachine, animBoolName, lookForPlayerSO) {
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
        else if (isAllTurnsTimeDone) {
            _finiteStateMachine.ChangeState(redArcher.moveState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
