using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RL_MoveState : MoveState {

    private RedLuker redLuker;

    public RL_MoveState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, RedLuker redLuker) : base(entity, finiteStateMachine, animBoolName) {
        this.redLuker = redLuker;
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
        else if (isDetectingWall || !isDetectingLedge || isDetectingCeiling) {
            redLuker.idleState.SetFlipAfterIdle(true);
            _finiteStateMachine.ChangeState(redLuker.idleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
