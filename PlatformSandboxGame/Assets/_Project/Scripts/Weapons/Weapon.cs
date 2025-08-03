using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected Player Player;

    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected PlayerAttackState playerAttackState;


    protected int attackCounter;

    protected virtual void Awake() {
        baseAnimator = transform.Find(GameObjectNameConstants.BASE).GetComponent<Animator>();
        weaponAnimator = transform.Find(GameObjectNameConstants.WEAPON).GetComponent<Animator>();

        gameObject.SetActive(false);
    }
    protected virtual void Start() {

    }

    public virtual void EnterWeapon() {
        gameObject.SetActive(true);

        //every speed is a blow
        if (attackCounter >= weaponSO.amountOfAttacks) {
            attackCounter = 0;
        }

        baseAnimator.SetBool(PlayerStateConstants.ATTACK, true);
        weaponAnimator.SetBool(PlayerStateConstants.ATTACK, true);

        baseAnimator.SetInteger(PlayerStateConstants.ATTACK_COUNTER, attackCounter);
        weaponAnimator.SetInteger(PlayerStateConstants.ATTACK_COUNTER, attackCounter);
    }

    public virtual void ExitWeapon() {
        baseAnimator.SetBool(PlayerStateConstants.ATTACK, false);
        weaponAnimator.SetBool(PlayerStateConstants.ATTACK, false);

        attackCounter++;
        gameObject.SetActive(false);
    }

    #region Animation Triggers

    public virtual void AnimationFinishTrigger() {
        playerAttackState.AnimationFinishTrigger();
    }

    public virtual void AnimationStartMovement() {
        playerAttackState.SetPlayerVelocity(weaponSO.movementSpeed[attackCounter]);
    }

    public virtual void AnimationFinishMovement() {
        playerAttackState.SetPlayerVelocity(0f);
    }

    public virtual void AnimationTurnOffFlipTrigger() {
        playerAttackState.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger() {
        playerAttackState.SetFlipCheck(true);
    }

    public virtual void AnimationActionTrigger() { }
    #endregion

    public void InitatelizeWeapon(PlayerAttackState playerAttackState) {
        this.playerAttackState = playerAttackState;
    }



}
