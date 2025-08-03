using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State {

    protected LookForPlayerSO lookForPlayerSO;

    protected bool turnImmediately;
    protected bool isPlayerInMinAgroRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;

    protected float lastTurnTime;

    protected int amountOfTurnDone;


    public LookForPlayerState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, LookForPlayerSO lookForPlayerSO) : base(entity, finiteStateMachine, animBoolName) {
        this.lookForPlayerSO = lookForPlayerSO;
    }

    public override void DoChecks() {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter() {
        base.Enter();

        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;

        lastTurnTime = startStateTime;
        amountOfTurnDone = 0;

        entity.SetVelocity(0f);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
        entity.SetVelocity(0f);

        if (turnImmediately) {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnDone++;
            turnImmediately = false;
        } else if (Time.time >= lastTurnTime + lookForPlayerSO.timeBetweenTurns && !isAllTurnsDone) {
            entity.Flip();
            lastTurnTime = Time.time;
            amountOfTurnDone++;
        }

        if (amountOfTurnDone >= lookForPlayerSO.amountOfTurns) {
            isAllTurnsDone = true;
        }

        if (Time.time >= lastTurnTime + lookForPlayerSO.timeBetweenTurns && isAllTurnsDone) {
            isAllTurnsTimeDone = true;
        }

    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public void SetTurnImmediately(bool flip) {
        turnImmediately = flip;
    }
}
