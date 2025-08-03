using UnityEngine;

public class ParticleManager : GenericPoolManager<ParticleData, ParticleController> {
    protected override string GetPoolID(ParticleData data) => data.particleID;
    protected override int GetInitialPoolSize(ParticleData data) => data.initialPoolSize;
    protected override float GetLifetime(ParticleData data) => data.lifetime;
    protected override GameObject GetPrefab(ParticleData data) => data.prefab;

    // Shortcut method for convenience
    public GameObject SpawnParticle(string particleID, Vector3 position, Quaternion rotation) {
        return SpawnObject(particleID, position, rotation);
    }
}
