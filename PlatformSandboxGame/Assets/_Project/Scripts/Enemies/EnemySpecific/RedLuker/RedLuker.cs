using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLuker : Entity {

    public RL_IdleState idleState {  get; private set; }
    public RL_MoveState moveState { get; private set; }
    public RL_PlayerDetectedState playerDetectedState { get; private set; }
    public RL_ChargeState chargeState { get; private set; }
    public RL_LookForPlayerState lookForPlayerState { get; private set; }
    public RL_MeleeAttackState meleeAttackState { get; private set; }
    public RL_StunState stunState { get; private set; }
    public RL_DeadState deadState { get; private set; }


    [SerializeField] private IdleSO idleSO;
    [SerializeField] private PlayerDetectedSO playerDetectedSO;
    [SerializeField] private ChargeStateSO chargeStateSO;
    [SerializeField] private LookForPlayerSO lookForPlayerSO;
    [SerializeField] private MeleeAttackSO meleeAttackSO;
    [SerializeField] private StunSO stunSO;
    [SerializeField] private DeadSO deadSO;


    [SerializeField] private Transform meleeAttackPosition;

    public override void Awake() {
        base.Awake();
    }

    protected override void Start() {
        base.Start();

        moveState = new RL_MoveState(this, FiniteStateMachine, EnemyStateConstants.MOVE, this);
        idleState = new RL_IdleState(this, FiniteStateMachine, EnemyStateConstants.IDLE, idleSO, this);
        playerDetectedState = new RL_PlayerDetectedState(this, FiniteStateMachine, EnemyStateConstants.PLAYER_DETECTED, playerDetectedSO, this);
        chargeState = new RL_ChargeState(this, FiniteStateMachine, EnemyStateConstants.CHARGE, chargeStateSO, this);
        lookForPlayerState = new RL_LookForPlayerState(this, FiniteStateMachine, EnemyStateConstants.LOOK_FOR_PLAYER, lookForPlayerSO, this);
        meleeAttackState = new RL_MeleeAttackState(this, FiniteStateMachine, EnemyStateConstants.MELEE_ATTACK, meleeAttackPosition, meleeAttackSO, this);
        stunState = new RL_StunState(this, FiniteStateMachine, EnemyStateConstants.STUN, stunSO, this);
        deadState = new RL_DeadState(this, FiniteStateMachine, EnemyStateConstants.DEAD, deadSO.particleBloodData, deadSO.particleChunkData, this);
            

        FiniteStateMachine.Initialize(moveState);
    }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackSO.attackRadius);
    }
}
