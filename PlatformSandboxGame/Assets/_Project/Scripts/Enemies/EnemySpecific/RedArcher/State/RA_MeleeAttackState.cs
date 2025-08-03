using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RA_MeleeAttackState : MeleeAttackState {

    private RedArcher redArcher;
    public RA_MeleeAttackState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, Transform attackPosition, MeleeAttackSO meleeAttackSO, RedArcher redArcher) : base(entity, finiteStateMachine, animBoolName, attackPosition, meleeAttackSO) {
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

    public override void FinishAttack() {
        base.FinishAttack();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (isAnimationFinished) {
            if (isPlayerInMinAgroRange) {
                _finiteStateMachine.ChangeState(redArcher.playerDetectedState);
            }
            else if (!isPlayerInMinAgroRange) {
                _finiteStateMachine.ChangeState(redArcher.lookForPlayerState);
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
