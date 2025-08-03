using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class DodgeState : State {

    protected DodgeSO dodgeSO;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMaxAgroRange;
    protected bool isGrounded;
    protected bool isDodgeOver;
    protected bool isDetectingWallBehind;
    protected bool isDetectingLedgeBehind;
    protected bool flipAfterDodge;

    public DodgeState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, DodgeSO dodgeSO) : base(entity, finiteStateMachine, animBoolName) {
        this.dodgeSO = dodgeSO;
    }

    public override void DoChecks() {
        base.DoChecks();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isGrounded = entity.CheckGround();
        isDetectingWallBehind = entity.CheckWallBehind();
        isDetectingLedgeBehind = entity.CheckLedgeBehind();
    }

    public override void Enter() {
        base.Enter();
      
        isDodgeOver = false;
        flipAfterDodge = false;

        if (isDetectingWallBehind || !isDetectingLedgeBehind) {
            entity.SetVelocity(dodgeSO.dodgeSpeed, dodgeSO.dodgeAngle, entity.FacingDirection);
        }
        else {
            entity.SetVelocity(dodgeSO.dodgeSpeed, dodgeSO.dodgeAngle, -entity.FacingDirection);
        }
    }

    public override void Exit() {
        base.Exit();

    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        if (Time.time >= startStateTime + dodgeSO.dodgeTime && isGrounded) {
            isDodgeOver = true;
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
    
    public void SetFlipAfterDodge(bool flip) {
        flipAfterDodge = flip;
    }
}
