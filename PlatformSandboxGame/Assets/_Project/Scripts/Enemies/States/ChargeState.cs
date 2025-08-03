 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State {

    protected ChargeStateSO chargeStateSO;

    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;

    public ChargeState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, ChargeStateSO chargeStateSO) : base(entity, finiteStateMachine, animBoolName) {
        this.chargeStateSO = chargeStateSO;
    }

    public override void DoChecks() {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter() {
        base.Enter();

        isChargeTimeOver = false;
        entity.SetVelocity(chargeStateSO.chargeSpeed);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        entity.SetVelocity(chargeStateSO.chargeSpeed);

        if (Time.time >= startStateTime + chargeStateSO.chargeTime) {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
