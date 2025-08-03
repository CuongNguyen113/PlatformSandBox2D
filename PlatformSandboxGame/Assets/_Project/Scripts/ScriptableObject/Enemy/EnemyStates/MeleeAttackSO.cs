using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackSO", menuName = "Scriptable Object/State SO/Melee Attack State")]
public class MeleeAttackSO : ScriptableObject
{
    public float attackRadius = 0.5f;
    public LayerMask whatIsPlayer;

    public Vector2 knockbackAngle = Vector2.one;
    public float knockbackStrength = 10f;

}
