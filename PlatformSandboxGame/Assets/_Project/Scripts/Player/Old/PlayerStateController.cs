using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMovementState {
    Idling = 0,
    Walking = 1,
    Srpinting = 2,
    WallSliding = 3,
    LedgeClimbing = 4,
    Dashing = 5,
    Jumping = 6,
    Falling = 7,
    Grounded = 8,
    Dead = 9
}


public enum PlayerCombatState {
    None = 0,
    Attacking = 1,
}
[DefaultExecutionOrder(-2)]
public class PlayerStateController : MonoBehaviour
{
    public event Action OnFinishAttack;
    private GameManager gameManager;

    [SerializeField] private GameObject deathChunkParticle, deathBloodParticle;
    [field: SerializeField] public PlayerMovementState CurrentPlayerMovementState { get; private set; } = PlayerMovementState.Idling;
    [field: SerializeField] public PlayerCombatState CurrentPlayerCombatState { get; private set; } = PlayerCombatState.None;

    public bool isCombatState {  get; private set; }

    private void Awake() {
        gameManager = GameObject.Find(GameObjectNameConstants.GAME_MANAGER).GetComponent<GameManager>();
        isCombatState = false;
    }

    public void SetPlayerMovementState(PlayerMovementState newState) {

        if (isCombatState && IsPriorityMovement(newState)) {
            //Cancel attack playerAttackState
            SetPlayerCombatState(PlayerCombatState.None);
            OnFinishAttack();
        }
        isCombatState = false;
        CurrentPlayerMovementState = newState; 
    }

    private bool IsPriorityMovement(PlayerMovementState newState) {
        return newState == PlayerMovementState.Jumping
            || newState == PlayerMovementState.WallSliding;
    }

    public void SetPlayerCombatState(PlayerCombatState newState) {
        if (CurrentPlayerCombatState != newState) {
            CurrentPlayerCombatState = newState;
            isCombatState = true;
        }
    }


    private void Die() {
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        gameManager.Respawn();
        Destroy(gameObject);
    }
}
