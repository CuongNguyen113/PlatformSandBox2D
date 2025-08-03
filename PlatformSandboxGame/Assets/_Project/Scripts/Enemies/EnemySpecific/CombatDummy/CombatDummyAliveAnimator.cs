using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class CombatDummyAnimator : MonoBehaviour
{
    private Animator _aliveAnimator;
    private GameObject aliveGO;
    private CombatDummyController _combatDummmyController;

    private void Awake() {

        aliveGO = transform.Find(TagConstants.ALIVE).gameObject;
        _combatDummmyController = GetComponent<CombatDummyController>();
        _aliveAnimator = aliveGO.GetComponent<Animator>();

        _combatDummmyController.OnSetAnimatorBool += SetAnimatorBool;
        _combatDummmyController.OnSetAnimatorTrigger += SetAnimatorTrigger;
    }


    private void Update() {
        
    }


    private void SetAnimatorBool(string paramName, bool value) {
        _aliveAnimator.SetBool(paramName, value);
    }

    private void SetAnimatorTrigger(string trigger) {
        _aliveAnimator.SetTrigger(trigger);
    }
}
