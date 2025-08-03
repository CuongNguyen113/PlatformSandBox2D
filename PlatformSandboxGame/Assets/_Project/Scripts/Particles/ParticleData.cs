using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewParticleData", menuName = "Particles/Particle Data")]
public class ParticleData : ScriptableObject {
    public string particleID;
    public GameObject prefab;
    public int initialPoolSize = 10;
    public float lifetime = 1f;
}