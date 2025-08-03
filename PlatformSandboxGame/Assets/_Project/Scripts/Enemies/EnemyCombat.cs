using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyCombat : MonoBehaviour, IDamageable, IKnockbackable {

    [SerializeField] private float maxKnockbackTime = 0.1f;

    private bool isKnockbackActive;
    private float knockbackStartTime;

    [SerializeField] private Entity Entity;
    [SerializeField] private StatsController statsController;

    private void Update() {
        CheckKnockback();
    }

    public void Damage(int amount) {
        statsController.DecreaseCurrentHealth(amount);

        var particle = ParticleManager.Instance.SpawnObject(
            "RedHitParticle",
            transform.position,
            Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f))
        );
    }

    public void Knockback(Vector2 angle, float strength, int direction) {
        Entity.SetVelocity(strength, angle, direction);
        Entity.CanSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback() {
        if (isKnockbackActive && (Entity.RB.velocity.y <= 0.01f && Entity.CheckGround()) || Time.time >= knockbackStartTime + maxKnockbackTime) {
            isKnockbackActive = false;
            Entity.CanSetVelocity = true;
        }
    }
}
