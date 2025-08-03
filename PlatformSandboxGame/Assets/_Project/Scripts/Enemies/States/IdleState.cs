using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State {

    protected IdleSO idleSO;

    protected bool flipAfterIdle;
    protected bool flipBeforeIdle;
    protected bool isIdleTimerOver;
    protected bool isPlayerInMinAgroRange;
    protected bool isDetectingWall;
    protected bool isDetectingLedge;

    protected float idleTime;

    public IdleState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, IdleSO idleSO) : base(entity, finiteStateMachine, animBoolName) {
        this.idleSO = idleSO;
    }

    public override void DoChecks() {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDetectingWall = entity.CheckWall();
        isDetectingLedge = entity.CheckLedge();
    }

    public override void Enter() {
        if (flipBeforeIdle) {
            entity.Flip();
        }
        base.Enter();

        entity.SetVelocity(0.0f);
        isIdleTimerOver = false;
        SetRandomIdleTime();
    }

    public override void Exit() {
        base.Exit();

        if (flipAfterIdle) {
            entity.Flip();
        }
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        entity.SetVelocity(0.0f);

        if (Time.time >= startStateTime + idleTime) {
            isIdleTimerOver = true;
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public void SetFlipAfterIdle(bool flip) {
        flipAfterIdle = flip;
    }

    public void SetFlipBeforeIdle(bool flip) {
        flipBeforeIdle = flip;
    }

    private void SetRandomIdleTime() {
        idleTime = Random.Range(idleSO.minIdleTime, idleSO.maxIdleTime);
    }
}
