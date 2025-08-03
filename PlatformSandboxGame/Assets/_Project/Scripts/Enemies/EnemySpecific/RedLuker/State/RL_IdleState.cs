using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RL_IdleState : IdleState {

    private RedLuker redLuker;

    public RL_IdleState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, IdleSO idleSO, RedLuker redLuker) : base(entity, finiteStateMachine, animBoolName, idleSO) {
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
            if (isDetectingLedge) {
                _finiteStateMachine.ChangeState(redLuker.playerDetectedState);
            }
        }
        else if (isIdleTimerOver) {
            _finiteStateMachine.ChangeState(redLuker.moveState);
        }


         
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
