using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedAttackState : AttackState {
 
    protected RangedAttackSO rangedAttackSO;

    protected GameObject projectile;
    protected ProjectileController projectileController;



    public RangedAttackState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, Transform attackPosition, RangedAttackSO rangedAttackSO) : base(entity, finiteStateMachine, animBoolName, attackPosition) {
        this.rangedAttackSO = rangedAttackSO;
    }

    public override void DoChecks() {
        base.DoChecks();
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void FinishAttack() {
        base.FinishAttack();
    }

    public override void LogicUpdate() {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack() {
        base.TriggerAttack();

        projectile = GameObject.Instantiate(rangedAttackSO.projectile, attackPosition.position, attackPosition.rotation);
        projectileController = projectile.GetComponent<ProjectileController>();
        projectileController.FireProjectile(rangedAttackSO.projectileSpeed, rangedAttackSO.projectileTravelDistance, rangedAttackSO.projectileDamage);
    }
}
