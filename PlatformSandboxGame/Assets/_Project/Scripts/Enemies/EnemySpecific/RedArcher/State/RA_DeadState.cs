using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RA_DeadState : DeadState {

    private RedArcher redArcher;
    public RA_DeadState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, ParticleData particleBloodData, ParticleData particleChunkData, RedArcher redArcher) : base(entity, finiteStateMachine, animBoolName, particleBloodData, particleChunkData) {
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
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}
