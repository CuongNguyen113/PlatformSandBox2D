using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RedArcher : Entity
{

    public RA_MoveState moveState {  get; private set; }
    public RA_IdleState idleState { get; private set; }
    public RA_PlayerDetectedState playerDetectedState { get; private set; }
    public RA_MeleeAttackState meleeAttackState { get; private set; }
    public RA_LookForPlayerState lookForPlayerState { get; private set; }
    public RA_StunState stunState { get; private set; }
    public RA_DeadState deadState { get; private set; }
    public RA_DodgeState dodgeState { get; private set; }
    public RA_RangedAttackState rangedAttackState { get; private set; }

    [SerializeField] private IdleSO idleSO;
    [SerializeField] private PlayerDetectedSO playerDetectedSO;
    [SerializeField] private MeleeAttackSO meleeAttackSO;
    [SerializeField] private LookForPlayerSO lookForPlayerSO;
    [SerializeField] private StunSO stunSO;
    [SerializeField] private DeadSO deadSO;
    [SerializeField] private RangedAttackSO rangedAttackSO;
    public DodgeSO dodgeSO;

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangedAttackPosition;    

    public override void Awake() {
        base.Awake();
    }

    protected override void Start() {
        base.Start();

        moveState = new RA_MoveState(this, FiniteStateMachine, EnemyStateConstants.MOVE, this);
        idleState = new RA_IdleState(this, FiniteStateMachine, EnemyStateConstants.IDLE, idleSO, this);
        playerDetectedState = new RA_PlayerDetectedState(this, FiniteStateMachine, EnemyStateConstants.PLAYER_DETECTED, playerDetectedSO, this);
        meleeAttackState = new RA_MeleeAttackState(this, FiniteStateMachine, EnemyStateConstants.MELEE_ATTACK, meleeAttackPosition, meleeAttackSO, this);
        lookForPlayerState = new RA_LookForPlayerState(this, FiniteStateMachine, EnemyStateConstants.LOOK_FOR_PLAYER, lookForPlayerSO, this);
        stunState = new RA_StunState(this, FiniteStateMachine, EnemyStateConstants.STUN, stunSO, this);
        deadState = new RA_DeadState(this, FiniteStateMachine, EnemyStateConstants.DEAD, deadSO.particleBloodData, deadSO.particleChunkData, this);
        dodgeState = new RA_DodgeState(this, FiniteStateMachine, EnemyStateConstants.DODGE, dodgeSO, this);
        rangedAttackState = new RA_RangedAttackState(this, FiniteStateMachine, EnemyStateConstants.RANGED_ATTACK, rangedAttackPosition, rangedAttackSO, this);

        FiniteStateMachine.Initialize(moveState);
    }

    protected override void Update() {
        base.Update();

        Animator.SetFloat(PlayerAnimationConstants.Y_VELOCITY, RB.velocity.y);
    }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackSO.attackRadius);
    }
}
