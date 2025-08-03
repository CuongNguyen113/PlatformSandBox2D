using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeSO", menuName = "Scriptable Object/State SO/Charge State")]
public class ChargeStateSO : ScriptableObject
{
    public float chargeSpeed = 6f;

    public float chargeTime = 0.5f;
}
