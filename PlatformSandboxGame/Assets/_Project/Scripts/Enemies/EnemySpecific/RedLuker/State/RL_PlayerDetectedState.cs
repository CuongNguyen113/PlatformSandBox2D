using  System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RL_PlayerDetectedState : PlayerDetectedState {

    private RedLuker redLuker;
    public RL_PlayerDetectedState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, PlayerDetectedSO playerDetectedSO, RedLuker redLuker) : base(entity, finiteStateMachine, animBoolName, playerDetectedSO) {
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

        if (performCloseRangeAction) {
            _finiteStateMachine.ChangeState(redLuker.meleeAttackState);
        }
        else if (performLongRangeAction) {
            _finiteStateMachine.ChangeState(redLuker.chargeState);
        }
        else if (!isPlayerInMaxArgoRange) {
            _finiteStateMachine.ChangeState(redLuker.lookForPlayerState);
        }
        else if (!isDetectingLedge) {
            _finiteStateMachine.ChangeState(redLuker.idleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
