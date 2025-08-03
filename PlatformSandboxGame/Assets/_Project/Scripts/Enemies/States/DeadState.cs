using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State {

    ParticleData particleBloodData;
    ParticleData particleChunkData;

    public DeadState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, ParticleData particleBloodData, ParticleData particleChunkData) : base(entity, finiteStateMachine, animBoolName) {
        this.particleBloodData = particleBloodData;
        this.particleChunkData = particleChunkData;
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();

        ParticleManager.Instance.SpawnObject(
            particleBloodData.particleID,
            entity._aliveGO.transform.position,
            particleBloodData.prefab.transform.rotation
        );

        ParticleManager.Instance.SpawnObject(
            particleChunkData.particleID,
            entity._aliveGO.transform.position,
            particleChunkData.prefab.transform.rotation
        );

        entity.gameObject.SetActive(false);
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
