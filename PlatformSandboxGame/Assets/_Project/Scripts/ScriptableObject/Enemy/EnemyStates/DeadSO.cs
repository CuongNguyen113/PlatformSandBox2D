using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newDeadSO", menuName = "Scriptable Object/State SO/Dead State")]
public class DeadSO : ScriptableObject {
    public ParticleData particleBloodData;
    public ParticleData particleChunkData;
}
