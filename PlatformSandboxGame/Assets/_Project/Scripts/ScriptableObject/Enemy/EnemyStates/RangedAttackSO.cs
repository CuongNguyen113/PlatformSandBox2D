using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newRangedAttackSO", menuName = "Scriptable Object/State SO/Ranged Attack State")]
public class RangedAttackSO : ScriptableObject
{
    public GameObject projectile;
    public int projectileDamage = 10;
    public float projectileSpeed = 12f;
    public float projectileTravelDistance = 10f; 

}
