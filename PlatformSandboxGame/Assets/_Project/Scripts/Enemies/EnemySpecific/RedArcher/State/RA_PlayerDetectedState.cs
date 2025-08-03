using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RA_PlayerDetectedState : PlayerDetectedState {

    private RedArcher redArcher;
    private Coroutine delayedStateChangeCoroutine;

    public RA_PlayerDetectedState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, PlayerDetectedSO playerDetectedSO, RedArcher redArcher) : base(entity, finiteStateMachine, animBoolName, playerDetectedSO) {
        this.redArcher = redArcher;
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
        // Hủy bỏ coroutine nếu đang chạy khi playerAttackState được enter lại
        if (delayedStateChangeCoroutine != null) {
            redArcher.StopCoroutine(delayedStateChangeCoroutine);
            delayedStateChangeCoroutine = null;
        }
    }

    public override void Exit() {
        base.Exit();
        // Hủy bỏ coroutine khi thoát playerAttackState
        if (delayedStateChangeCoroutine != null) {
            redArcher.StopCoroutine(delayedStateChangeCoroutine);
            delayedStateChangeCoroutine = null;
        }
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (performCloseRangeAction) {
            if (Time.time >= redArcher.dodgeState.startStateTime + redArcher.dodgeSO.dodgeCooldown) {

                delayedStateChangeCoroutine ??= redArcher.StartCoroutine(DelayedDodgeStateChange());
            } else {
                _finiteStateMachine.ChangeState(redArcher.meleeAttackState);
            }
        } else if (performLongRangeAction && !isDetectingWallBehind) {
            _finiteStateMachine.ChangeState(redArcher.rangedAttackState);
        } else if (isDetectingWallBehind) {
            _finiteStateMachine.ChangeState(redArcher.moveState);
        } else if (!isPlayerInMaxArgoRange) {
            _finiteStateMachine.ChangeState(redArcher.lookForPlayerState);
        }
    }

    private IEnumerator DelayedDodgeStateChange() {

        float delayTime = redArcher.dodgeSO.dodgeDelayTime;
        yield return new WaitForSeconds(delayTime);


        _finiteStateMachine.ChangeState(redArcher.dodgeState);

        // Reset coroutine reference
        delayedStateChangeCoroutine = null;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }
}