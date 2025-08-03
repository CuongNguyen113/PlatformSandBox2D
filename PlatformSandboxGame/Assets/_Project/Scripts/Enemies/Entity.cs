using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Entity : MonoBehaviour {
    public int FacingDirection { get; private set; }
    public FiniteStateMachine FiniteStateMachine { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Animator Animator { get; private set; }
    public GameObject _aliveGO { get; private set; }
    public AnimationToStateMachine AnimToStateMachine { get; private set; }
    public StatsController StatsController { get; private set; }
    public int LastDamageDirecion { get; private set; }


    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform behindLedgeCheck;
    [SerializeField] private Transform CeilingCheck;

    [SerializeField] private EntitySO entitySO;
    [SerializeField] private StunMechanicSO stunMechanicSO;

    private Vector2 velocityWorkspace;
    private Vector2 gizmosDirection;
    public Vector2 CurrentVelocity { get; private set; }

    private float lastDamageTime;

    private int currentStunResistance;

    protected bool isStunned;
    protected bool isDead;
    public bool CanSetVelocity { get; set; }

    private static readonly Collider2D[] OverlapBuffer = new Collider2D[10];


    public virtual void Awake() {

        _aliveGO = transform.Find(TagConstants.ALIVE).gameObject;
        RB = _aliveGO.GetComponent<Rigidbody2D>();
        Animator = _aliveGO.GetComponent<Animator>();

        FiniteStateMachine = new FiniteStateMachine();
        AnimToStateMachine = _aliveGO.GetComponent<AnimationToStateMachine>();
        StatsController = gameObject.GetComponent<StatsController>();

    }

    protected virtual void Start() {
        FacingDirection = 1;
        CanSetVelocity = true;
        currentStunResistance = stunMechanicSO.stunResistance;
    }

    protected virtual void Update() {
        FiniteStateMachine.currentState.LogicUpdate();

        if (Time.time >= lastDamageTime + stunMechanicSO.stunRecoverTime) {
            ResetStunResistance();
        }
    }

    protected virtual void FixedUpdate() {
        FiniteStateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity) {
        velocityWorkspace.Set(FacingDirection * velocity, RB.velocity.y);
        SetFinalVelocity();
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction) {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.x * velocity);
        SetFinalVelocity();
    }
    private void SetFinalVelocity() {
        if (CanSetVelocity) {
            RB.velocity = velocityWorkspace;
            CurrentVelocity = velocityWorkspace;
        }
    }
    public virtual bool CheckWall() {
        return Physics2D.Raycast(wallCheck.position, _aliveGO.transform.right, entitySO.wallCheckDistance, entitySO.whatIsGround);
    }

    public virtual bool CheckWallBehind() {
        return Physics2D.Raycast(wallCheck.position, -_aliveGO.transform.right, entitySO.wallBehindCheckDistance, entitySO.whatIsGround);
    }

    public virtual bool CheckLedgeBehind() {
        return Physics2D.Raycast(behindLedgeCheck.position, Vector2.down, entitySO.ledgeCheckDistance, entitySO.whatIsGround);
    }

    public virtual bool CheckLedge() {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entitySO.ledgeCheckDistance, entitySO.whatIsGround);
    }

    public virtual bool CheckGround() {
        return Physics2D.OverlapCircle(groundCheck.position, entitySO.groundCheckRadius, entitySO.whatIsGround);
    }

    public bool CheckPlayerInMinAgroRange() => DetectPlayer(entitySO.minAgroDistance);
    public bool CheckPlayerInMaxAgroRange() => DetectPlayer(entitySO.maxAgroDistance);

    public virtual bool CheckPlayerInCloseRangeAction() {
        return Physics2D.Raycast(playerCheck.position, Vector2.right * FacingDirection, entitySO.closeRangeActionDistance, entitySO.whatIsPlayer);
    }
    public bool CheckForCeiling() {
        return Physics2D.OverlapCircle(CeilingCheck.position, entitySO.groundCheckRadius, entitySO.whatIsGround);
    }
    protected virtual void DamageHop(float velocity) {
        velocityWorkspace.Set(RB.velocity.x, velocity);
        SetFinalVelocity();
    }


    public virtual void ResetStunResistance() {
        isStunned = false; 
        currentStunResistance = stunMechanicSO.stunResistance;
    }

    public virtual void Flip() {
        FacingDirection *= -1;
        _aliveGO.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public bool DetectPlayer(float range, float maxHeightDifference = 2f) {
        // 1. Kiểm tra player trong phạm vi
        int hitCount = Physics2D.OverlapCircleNonAlloc(
            (Vector2)playerCheck.position,
            range,
            OverlapBuffer,
            entitySO.whatIsPlayer
        );

        if (hitCount == 0) return false;


        Transform player = OverlapBuffer[0].transform;
        Vector2 directionToPlayer = player.position - playerCheck.position;

        if (Vector2.Dot(directionToPlayer.normalized, Vector2.right * FacingDirection) < 0) {
            return false;
        }

        if (Mathf.Abs(directionToPlayer.y) > maxHeightDifference) {
            return false;
        }

        Vector2 horizontalDirection = new Vector2(directionToPlayer.x, 0).normalized;
        float horizontalDistance = Mathf.Abs(directionToPlayer.x);

        Debug.DrawRay(playerCheck.position, horizontalDirection * horizontalDistance, Color.red, 1f);

        RaycastHit2D hit = Physics2D.Raycast(
            playerCheck.position,
            horizontalDirection,
            horizontalDistance,
            entitySO.whatIsGround
        );

        return hit.collider == null;
    }
    protected virtual void OnDrawGizmos() {
        Vector2 direction;

        if (FacingDirection != 0) {
            direction = Vector2.right * FacingDirection;
        } else {
            direction = Vector2.right;
        }

        Gizmos.color = Color.green;

        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(entitySO.wallCheckDistance * direction));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(entitySO.ledgeCheckDistance * Vector2.down));



        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(entitySO.minAgroDistance * direction), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(entitySO.maxAgroDistance * direction), 0.2f);


        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(entitySO.closeRangeActionDistance * direction), 0.2f); 
        
        Gizmos.color = Color.black;
        Gizmos.DrawLine(behindLedgeCheck.position, behindLedgeCheck.position + (Vector3)(entitySO.ledgeCheckDistance * Vector2.down));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position - (Vector3)(entitySO.wallCheckDistance * direction));

    }
}