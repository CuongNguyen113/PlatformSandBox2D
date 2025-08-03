using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State {

    protected PlayerDetectedSO playerDetectedSO;

    protected bool isPlayerInMinArgoRange;
    protected bool isPlayerInMaxArgoRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;
    protected bool isDetectingWallBehind;

    public PlayerDetectedState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, PlayerDetectedSO playerDetectedSO) : base(entity, finiteStateMachine, animBoolName) {
        this.playerDetectedSO = playerDetectedSO; 
    }

    public override void DoChecks() {
        base.DoChecks();

        isPlayerInMinArgoRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxArgoRange = entity.CheckPlayerInMaxAgroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isDetectingLedge = entity.CheckLedge();
        isDetectingWallBehind = entity.CheckWallBehind();
    }

    public override void Enter() {
        base.Enter();

        performLongRangeAction = false;
        entity.SetVelocity(0f);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        entity.SetVelocity(0f);

        if (Time.time >= startStateTime + playerDetectedSO.longRangeActionTime) {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
