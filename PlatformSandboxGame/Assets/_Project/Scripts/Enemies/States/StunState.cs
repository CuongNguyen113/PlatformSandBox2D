using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State {

    protected StunSO stunSO;

    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingWallBehind;

    public StunState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, StunSO stunSO) : base(entity, finiteStateMachine, animBoolName) {
        this.stunSO = stunSO;
    }

    public override void DoChecks() {
        base.DoChecks();

        isGrounded = entity.CheckGround();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDetectingWallBehind = entity.CheckWallBehind();
    }

    public override void Enter() {
        base.Enter();

        isStunTimeOver = false;
        isMovementStopped = false;
        entity.SetVelocity(stunSO.stunKnockbackSpeed, stunSO.stunKnockbackAngle, entity.LastDamageDirecion);
    }

    public override void Exit() {
        base.Exit();

        entity.ResetStunResistance();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (Time.time >= startStateTime + stunSO.stunTime) {
            isStunTimeOver = true;
        }

        if (isGrounded && Time.time >= startStateTime + stunSO.stunKnockbackTime && !isMovementStopped) {
            isMovementStopped = false;
            entity.SetVelocity(0f);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
