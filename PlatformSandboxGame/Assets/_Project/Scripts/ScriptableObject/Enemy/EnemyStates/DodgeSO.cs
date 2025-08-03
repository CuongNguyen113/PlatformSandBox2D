using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDodgeSO", menuName = "Scriptable Object/State SO/Dodge State")]
public class DodgeSO : ScriptableObject
{
    public float dodgeSpeed = 10f;
    public float dodgeTime = 0.2f;
    public float dodgeCooldown = 2f;
    public float dodgeDelayTime = 10f;
    public Vector2 dodgeAngle;
}
