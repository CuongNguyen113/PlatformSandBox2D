using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RL_DeadState : DeadState {

    RedLuker redLuker;
    public RL_DeadState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, ParticleData particleBloodData, ParticleData particleChunkData, RedLuker redLuker) : base(entity, finiteStateMachine, animBoolName, particleBloodData, particleChunkData) {
        this.redLuker = redLuker;
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
