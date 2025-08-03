using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleSO", menuName = "Scriptable Object/State SO/Idle State")]
public class IdleSO : ScriptableObject {
    public float minIdleTime = 2f;
    public float maxIdleTime = 10f;
}
