using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RL_MeleeAttackState : MeleeAttackState {

    private RedLuker redLuker;

    public RL_MeleeAttackState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, Transform attackPosition, MeleeAttackSO meleeAttackSO, RedLuker redLuker) : base(entity, finiteStateMachine, animBoolName, attackPosition, meleeAttackSO) {
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

    public override void FinishAttack() {
        base.FinishAttack();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (isAnimationFinished) {
            if (isPlayerInMinAgroRange) {
                _finiteStateMachine.ChangeState(redLuker.playerDetectedState);
            } else {
                _finiteStateMachine.ChangeState(redLuker.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack() {
        base.TriggerAttack();
    }
}
