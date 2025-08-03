using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponAnimationToWeapon : MonoBehaviour {
    private Weapon weapon;

    private void Start() {
        weapon = GetComponentInParent<Weapon>();
    }

    private void AnimationFinishTrigger() {
        weapon.AnimationFinishTrigger();
    }

    private void AnimationStartMovementTrigger() {
        weapon.AnimationStartMovement();
    }

    private void AnimationFinishMovementTrigger() {
        weapon.AnimationFinishMovement();
    }

    private void AnimationTurnOffFlipTrigger() {
        weapon.AnimationTurnOffFlipTrigger();
    }

    private void AnimationTurnOnFlipTrigger() {
        weapon.AnimationTurnOnFlipTrigger();
    }

    private void AnimationActionTrigger() {
        weapon.AnimationActionTrigger();
    }
}
