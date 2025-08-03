using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLookForPlayerSO", menuName = "Scriptable Object/State SO/Look For Player State")]
public class LookForPlayerSO : ScriptableObject
{
    public int amountOfTurns = 2;
    public float timeBetweenTurns = 0.75f;

}
