using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected Transform attackPosition;

    protected bool isAnimationFinished;
    protected bool isPlayerInMinAgroRange;
    protected bool performCloseRangeAttack;

    public AttackState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, Transform attackPosition) : base(entity, finiteStateMachine, animBoolName) {
        this.attackPosition = attackPosition;
    }

    public override void DoChecks() {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        performCloseRangeAttack = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter() {
        base.Enter();

        entity.AnimToStateMachine.attackState = this;
        isAnimationFinished = false;
        entity.SetVelocity(0f);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        entity.SetVelocity(0f);

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public virtual void TriggerAttack() {

    }

    public virtual void FinishAttack() {
        isAnimationFinished = true;
    }
}
