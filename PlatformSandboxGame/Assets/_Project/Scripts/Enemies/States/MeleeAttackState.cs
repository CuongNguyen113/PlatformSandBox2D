using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackState : AttackState {

    protected MeleeAttackSO meleeAttackSO;

    private int damage;

    public MeleeAttackState(Entity entity, FiniteStateMachine finiteStateMachine, string animBoolName, Transform attackPosition, MeleeAttackSO meleeAttackSO) : base(entity, finiteStateMachine, animBoolName, attackPosition) {
        this.meleeAttackSO = meleeAttackSO;

        damage = entity.StatsController.GetAttack();
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

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, meleeAttackSO.attackRadius, meleeAttackSO.whatIsPlayer);

        foreach (Collider2D collider in detectedObjects) {
            IDamageable damageable = collider.GetComponent<IDamageable>();

            if (damageable != null) {
                damageable.Damage(damage);
            }

            IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();
            if (knockbackable != null) {
                knockbackable.Knockback(meleeAttackSO.knockbackAngle, meleeAttackSO.knockbackStrength, entity.FacingDirection);
            }

        }

    }
}
