using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackDetails {

    public Vector2 position;
    public int damageAmount;
    public int stunDamageAmount;
}

[System.Serializable]
public struct WeaponAttackDetails {
    public string attackName;
    public float movementSpeed;
    public int damageAmount;

    public float knockbackStrength;
    public Vector2 knockbackAngle;
}
