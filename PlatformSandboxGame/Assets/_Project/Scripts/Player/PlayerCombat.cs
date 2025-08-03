using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, IDamageable, IKnockbackable {

    [SerializeField] private float maxKnockbackTime = 0.1f;

    private bool isKnockbackActive;
    private float knockbackStartTime;

    [SerializeField] private Player player;
    [SerializeField] private StatsController statsController;

    private void Update() {
        CheckKnockback();
    }
    public void Damage(int amount) {
        statsController.DecreaseCurrentHealth(amount);
        var particle = ParticleManager.Instance.SpawnObject(
            "WhiteHitParticle",
            transform.position,
            Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f))
        );
    }

    public void Knockback(Vector2 angle, float strenght, int direction) {
        player.SetVelocity(strenght, angle, direction);
    }

    private void CheckKnockback() {
        if (isKnockbackActive && (player.CurrentVelocity.y <= 0.01f && player.CheckIfGrounded()) || Time.time >= knockbackStartTime + maxKnockbackTime) {
            isKnockbackActive = false;
            player.CanSetVelocity = true;
        }
    }

}
