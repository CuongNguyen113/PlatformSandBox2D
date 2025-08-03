 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RA_MoveState : MoveState {

    private RedArcher redArcher;

    private StatsController statsController;
    public RA_MoveState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, RedArcher redArcher) : base(entity, finiteStateMachine, animBoolName) {
        this.redArcher = redArcher;
        statsController = entity.gameObject.GetComponent<StatsController>();
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

        if (isPlayerInMinAgroRange) {
            if (!isDetectingWallBehind) {
                _finiteStateMachine.ChangeState(redArcher.playerDetectedState);
            }
        }
        else if (isDetectingWall || !isDetectingLedge || isDetectingCeiling) {
            redArcher.idleState.SetFlipAfterIdle(true);
            _finiteStateMachine.ChangeState(redArcher.idleState);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
