using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {
    protected FiniteStateMachine _finiteStateMachine;
    protected Entity entity;

    public float startStateTime { get; protected set; }

    protected string animBoolName;

    public State(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName) {
        this.entity = entity;
        this._finiteStateMachine = finiteStateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() {
        startStateTime = Time.time;
        entity.Animator.SetBool(animBoolName, true);
        DoChecks();
    }

    public virtual void Exit() {
        entity.Animator.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() {

    }

    public virtual void PhysicsUpdate() {
        DoChecks();
    }


    public virtual void DoChecks() {

    }
}
