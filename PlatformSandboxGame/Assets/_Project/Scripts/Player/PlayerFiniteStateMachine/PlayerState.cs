using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinish;
    protected bool isExitingState;

    protected float startStateTime;

    private string animBoolName;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() {
        DoCheck();
        player.Animator.SetBool(animBoolName, true);
        startStateTime = Time.time;
        isAnimationFinish = false;
        isExitingState = false;
    }

    public virtual void Exit() {
        player.Animator.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate() {

    }

    public virtual void PhysicsUpdate() {
        DoCheck();
    }

    public virtual void DoCheck() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinish = true;

}
