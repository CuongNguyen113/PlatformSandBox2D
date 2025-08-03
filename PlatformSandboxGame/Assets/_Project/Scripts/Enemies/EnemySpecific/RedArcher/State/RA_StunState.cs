using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RA_StunState : StunState {

    private RedArcher redArcher;
    public RA_StunState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, StunSO stunSO, RedArcher redArcher) : base(entity, finiteStateMachine, animBoolName, stunSO) {
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

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (isStunTimeOver) {
            if (isPlayerInMinAgroRange) {
                if (!isDetectingWallBehind) {
                    _finiteStateMachine.ChangeState(redArcher.playerDetectedState);
                } 
                else {
                    _finiteStateMachine.ChangeState(redArcher.moveState);
                }
            }
            else {
                _finiteStateMachine.ChangeState(redArcher.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
