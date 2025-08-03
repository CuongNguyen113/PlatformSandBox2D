using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RL_ChargeState : ChargeState {

    private RedLuker redLuker;

    public RL_ChargeState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, ChargeStateSO chargeStateSO, RedLuker redLuker) : base(entity, finiteStateMachine, animBoolName, chargeStateSO) {
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

        if (performCloseRangeAction) {
            _finiteStateMachine.ChangeState(redLuker.meleeAttackState);
        }
         else if (!isDetectingLedge || isDetectingWall) {
            _finiteStateMachine.ChangeState(redLuker.lookForPlayerState);
        }
        else if (isChargeTimeOver) {
            if (isPlayerInMinAgroRange) {
                if (isDetectingLedge) {

                }
                _finiteStateMachine.ChangeState(redLuker.playerDetectedState);
            }
            else {
                _finiteStateMachine.ChangeState(redLuker.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
