using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEngine;

public class AggressiveWeapon : Weapon {

    protected AggressiveWeaponSO aggressiveWeaponSO;

    private List<IDamageable> detectedDamageables = new List<IDamageable>();
    private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();


    protected override void Awake() {
        base.Awake();

        if (weaponSO.GetType() ==  typeof(AggressiveWeaponSO)) {
            aggressiveWeaponSO = (AggressiveWeaponSO)weaponSO;
        } else {
            Debug.LogError("Wrong data for the weapon");
        }
    }

    public override void AnimationActionTrigger() {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }
    private void CheckMeleeAttack() {

        WeaponAttackDetails details = aggressiveWeaponSO.AttackDetails[attackCounter];

        foreach (IDamageable item in detectedDamageables.ToList()) {
            item.Damage(details.damageAmount);
        }
        foreach (IKnockbackable item in detectedKnockbackables.ToList()) {
            item.Knockback(details.knockbackAngle, details.knockbackStrength, Player.FacingDirection);
        }

    }

    public void AddToDetected(Collider2D collision) {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null) {
            detectedDamageables.Add(damageable);
        }
        IKnockbackable kbno = collision.GetComponent<IKnockbackable>();
        if (kbno != null) {
            detectedKnockbackables.Add(kbno);
        }
    }

    public void RemoveFromDetected(Collider2D collision) {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null) {
            detectedDamageables.Remove(damageable);
        }

        IKnockbackable kbno = collision.GetComponent<IKnockbackable>();
        if (kbno != null) {
            detectedKnockbackables.Remove(kbno);
        }
    }

}
