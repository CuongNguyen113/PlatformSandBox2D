using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Weapon")]
public class WeaponSO : ScriptableObject {
    

    public int amountOfAttacks {  get; protected set; }
    public float[] movementSpeed {  get; protected set; }
}
