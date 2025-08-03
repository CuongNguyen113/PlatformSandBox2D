using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State {

    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingWallBehind;
    protected bool isDetectingCeiling;

    public MoveState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) : base(entity, finiteStateMachine, animBoolName) {

    }

    public override void DoChecks() {
        base.DoChecks();

        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDetectingWallBehind = entity.CheckWallBehind();
        isDetectingCeiling = entity.CheckForCeiling();
    }

    public override void Enter() {
        base.Enter();

        entity.SetVelocity(entity.StatsController.GetMoveSpeed()); 
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        entity.SetVelocity(entity.StatsController.GetMoveSpeed());

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}