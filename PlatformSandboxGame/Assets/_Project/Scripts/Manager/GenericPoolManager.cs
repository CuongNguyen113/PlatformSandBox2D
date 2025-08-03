using System.Collections.Generic;
using UnityEngine;

public abstract class GenericPoolManager<TData, TController> : MonoBehaviour
    where TData : class
    where TController : Component, IPoolable {
    [System.Serializable]
    public class ObjectPool {
        public TData data;
        public Transform parent;
        public Queue<GameObject> inactive = new();
        public List<GameObject> active = new(32);
    }

    private static GenericPoolManager<TData, TController> _instance;
    public static GenericPoolManager<TData, TController> Instance => _instance;

    [SerializeField] protected TData[] _poolDatas;
    protected Dictionary<string, ObjectPool> _pools = new();
    protected Dictionary<GameObject, ObjectPool> _objectToPoolMap = new();

    protected virtual void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        InitializePools();
    }

    protected virtual void InitializePools() {
        foreach (var data in _poolDatas) {
            var poolID = GetPoolID(data);
            var pool = new ObjectPool {
                data = data,
                parent = new GameObject($"{poolID}_Pool").transform
            };

            pool.parent.SetParent(transform);

            for (int i = 0; i < GetInitialPoolSize(data); i++) {
                var obj = CreateObject(data, pool.parent);
                pool.inactive.Enqueue(obj);
            }

            _pools.Add(poolID, pool);
        }
    }

    protected virtual GameObject CreateObject(TData data, Transform parent) {
        var prefab = GetPrefab(data);
        var obj = Instantiate(prefab, parent);
        obj.SetActive(false);

        if (!obj.TryGetComponent(out TController controller)) {
            controller = obj.AddComponent<TController>();
        }

        controller.Initialize(GetPoolID(data), GetLifetime(data));
        return obj;
    }

    public virtual GameObject SpawnObject(string poolID, Vector3 position, Quaternion rotation) {
        if (!_pools.TryGetValue(poolID, out var pool)) {
            Debug.LogError($"Pool {poolID} not found");
            return null;
        }

        GameObject obj;
        if (pool.inactive.Count > 0) {
            obj = pool.inactive.Dequeue();
        } else {
            obj = CreateObject(pool.data, pool.parent);
            Debug.Log($"Expanding pool for {poolID}");
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        pool.active.Add(obj);
        _objectToPoolMap.Add(obj, pool);

        var controller = obj.GetComponent<TController>();
        controller.OnSpawn();

        return obj;
    }

    public virtual void ReturnObject(GameObject obj) {
        if (!_objectToPoolMap.TryGetValue(obj, out var pool)) {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        pool.active.Remove(obj);
        pool.inactive.Enqueue(obj);
        _objectToPoolMap.Remove(obj);
    }

    protected virtual void LateUpdate() {
        foreach (var pool in _pools.Values) {
            for (int i = pool.active.Count - 1; i >= 0; i--) {
                var obj = pool.active[i];
                var controller = obj.GetComponent<TController>();

                if (controller.ShouldReturnToPool()) {
                    ReturnObject(obj);
                }
            }
        }
    }

    // Abstract methods to be implemented by child classes
    protected abstract string GetPoolID(TData data);
    protected abstract int GetInitialPoolSize(TData data);
    protected abstract float GetLifetime(TData data);
    protected abstract GameObject GetPrefab(TData data);
}

public interface IPoolable {
    void Initialize(string poolID, float lifetime);
    void OnSpawn();
    bool ShouldReturnToPool();
}