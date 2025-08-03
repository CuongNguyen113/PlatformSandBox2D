using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStunMechanicSO", menuName = "Scriptable Object/Mechanic SO/Stun Mechanic SO")]
public class StunMechanicSO : ScriptableObject
{
    public int stunResistance = 3;
    public float stunRecoverTime = 2f;
}
