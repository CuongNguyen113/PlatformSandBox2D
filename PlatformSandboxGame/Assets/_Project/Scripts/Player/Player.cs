using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {
    #region State Variable
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState {  get; private set; }

    [SerializeField] private PlayerData playerData;
    #endregion

    #region Components
    public Animator Animator { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public Transform dashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    #endregion

    #region Check Transforms

    [SerializeField] private Transform GroundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform LedgeCheck;
    [SerializeField] private Transform CeilingCheck;


    #endregion

    #region Other Variable
    public Vector2 CurrentVelocity { get; private set; }

    private Vector2 workspace;

    public const float tolerancesLedgeCLimb = 0.015f;
    public int FacingDirection { get; private set; }
    public bool CanSetVelocity {  get; set; }
    
    #endregion

    #region Unity Callback Functions

    private void Awake() {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, PlayerStateConstants.IDLE);
        MoveState = new PlayerMoveState(this, StateMachine, playerData, PlayerStateConstants.MOVE);
        JumpState = new PlayerJumpState(this, StateMachine, playerData, PlayerStateConstants.IN_AIR);
        InAirState = new PlayerInAirState(this, StateMachine, playerData, PlayerStateConstants.IN_AIR);
        LandState = new PlayerLandState(this, StateMachine, playerData, PlayerStateConstants.LAND);
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, PlayerStateConstants.WALL_SLIDE);
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, PlayerStateConstants.WALL_GRAB);
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, PlayerStateConstants.WALL_CLIMB);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, PlayerStateConstants.IN_AIR);
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, PlayerStateConstants.LEDGE_CLIMB_STATE);
        DashState = new PlayerDashState(this, StateMachine, playerData, PlayerStateConstants.IN_AIR);
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, PlayerStateConstants.CROUCH_IDLE);
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, PlayerStateConstants.CROUCH_MOVE);
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, PlayerStateConstants.ATTACK);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, PlayerAnimationConstants.ATTACK);

        Animator = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        Rigidbody = GetComponent<Rigidbody2D>();
        MovementCollider = GetComponent<BoxCollider2D>();
        dashDirectionIndicator = transform.Find(GameObjectNameConstants.DASH_DIRECTION_INDICATOR);
        Inventory = GetComponent<PlayerInventory>();
    }

    private void Start() {

        StateMachine.Initialize(IdleState);

        FacingDirection = 1;

        PrimaryAttackState.SetWeapon(Inventory.weapons[(int)EWeapon.sword1]);

        CanSetVelocity = true;
    }

    private void Update() {
        CurrentVelocity = Rigidbody.velocity;

        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Set Functions
    public void SetVelocityZero() {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }
    public void SetVelocity(float velocity, Vector2 angle, int direction) {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction) {
        workspace = direction * velocity;
        SetFinalVelocity();
    }

    public void SetVelocityX(float xVelocity) {
        workspace.Set(xVelocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float yVelocity) {
        workspace.Set(CurrentVelocity.x, yVelocity);
        SetFinalVelocity();
    }

    private void SetFinalVelocity() {
        if (CanSetVelocity) {
            Rigidbody.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }

    #endregion

    #region Check Functions

    public bool CheckForCeiling() {
        return Physics2D.OverlapCircle(CeilingCheck.position, playerData.groundCheckDistance, playerData.whatIsGround);
    }
    public bool CheckIfGrounded(){
        return Physics2D.OverlapCircle(GroundCheck.position, playerData.groundCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall() {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge() {
        return Physics2D.Raycast(LedgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public bool CheckIfTouchingWallBack() {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }
    public void CheckIfShouldFlip(int movementXInput) {
        if (movementXInput != 0 && movementXInput != FacingDirection) {
            Flip();
        }
    }

    #endregion

    #region Other Functions
    public void SetColliderHeight(float height) {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }

    public Vector2 DetermineCornerPosition() {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDistance = xHit.distance;
        workspace.Set((xDistance + tolerancesLedgeCLimb) * FacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(LedgeCheck.position + (Vector3)(workspace), Vector2.down, LedgeCheck.position.y - wallCheck.position.y + tolerancesLedgeCLimb, playerData.whatIsGround);
        float yDistance = yHit.distance;

        workspace.Set(wallCheck.position.x + (xDistance * FacingDirection), LedgeCheck.position.y - yDistance);
        return workspace;
    }
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();


    public virtual void Flip() {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
