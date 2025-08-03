using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RL_LookForPlayerState : LookForPlayerState {

    private RedLuker redLuker;

    public RL_LookForPlayerState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, LookForPlayerSO lookForPlayerSO, RedLuker redLuker) : base(entity, finiteStateMachine, animBoolName, lookForPlayerSO) {
        this.redLuker = redLuker;
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
            _finiteStateMachine.ChangeState(redLuker.playerDetectedState);
        } 
        else if (isAllTurnsTimeDone) {
            _finiteStateMachine.ChangeState(redLuker.moveState);
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
