using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RL_StunState : StunState {

    private RedLuker redLuker;

    public RL_StunState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, StunSO stunSO, RedLuker redLuker) : base(entity, finiteStateMachine, animBoolName, stunSO) {
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

        if (isStunTimeOver) {
            if (performCloseRangeAction) {
                _finiteStateMachine.ChangeState(redLuker.meleeAttackState);
            }
            else if (isPlayerInMinAgroRange) {
                _finiteStateMachine.ChangeState(redLuker.chargeState);
            } else {
                redLuker.lookForPlayerState.SetTurnImmediately(true);
                _finiteStateMachine.ChangeState(redLuker.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
