using UnityEngine;

public class ParticleController : MonoBehaviour, IPoolable {
    private string _particleID;
    private float _lifetime;
    private float _spawnTime;
    private ParticleSystem _particleSystem;
    private bool _useParticleSystem;

    public void Initialize(string id, float lifetime) {
        _particleID = id;
        _lifetime = lifetime;
        _particleSystem = GetComponent<ParticleSystem>();
        _useParticleSystem = _particleSystem != null;

        if (_useParticleSystem) {
            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.None;
        }
    }

    public void OnSpawn() {
        _spawnTime = Time.time;
        if (_useParticleSystem) _particleSystem.Play();
    }

    public bool ShouldReturnToPool() {
        if (_useParticleSystem) {
            return !_particleSystem.IsAlive(true);
        }
        return Time.time - _spawnTime >= _lifetime;
    }
}