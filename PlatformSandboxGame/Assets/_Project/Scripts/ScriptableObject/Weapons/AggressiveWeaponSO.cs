using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAggressiveWeaponSO", menuName = "Scriptable Object/Weapon SO/Aggressive Weapon")]
public class AggressiveWeaponSO : WeaponSO
{
    [SerializeField] private WeaponAttackDetails[] weaponAttackDetails;
    public WeaponAttackDetails[] AttackDetails { get => weaponAttackDetails; private set => weaponAttackDetails = value; }

    private void OnEnable() {
        amountOfAttacks = weaponAttackDetails.Length;

        movementSpeed = new float[amountOfAttacks];

        for (int i = 0; i < amountOfAttacks; i++) {
            movementSpeed[i] = weaponAttackDetails[i].movementSpeed;
        }

    }
}
