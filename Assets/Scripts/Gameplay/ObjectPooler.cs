namespace ToughPath.Gameplay {
    using System.Collections.Generic;
    using UnityEngine;

    public class ObjectPooler: MonoBehaviour {

        #region FIELDS
        private static ObjectPooler __Instance;

        [SerializeField] private List<Pool> _Pools;
        [SerializeField] private Transform  _ObjectParent;

        private Dictionary<string, Queue<GameObject>> _PoolDictionary;
        #endregion

        #region PUBLIC METHODS
        public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation) {
            if (!_PoolDictionary.ContainsKey(name)) {
                return null;
            }

            if (_PoolDictionary[name].Count == 0) {
                foreach (Pool pool in _Pools) {
                    if (pool.Name == name) {
                        GameObject item = pool.Spawn();
                        item.transform.SetParent(_ObjectParent);
                        EnqueueObject(name, item);
                    }
                }
            }

            GameObject objectToSpawn = _PoolDictionary[name].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            return objectToSpawn;
        }

        public void EnqueueObject(string poolName, GameObject objectToEnqueue) {
            objectToEnqueue.SetActive(false);
            _PoolDictionary[poolName].Enqueue(objectToEnqueue);
        }
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                return;
            }

            _PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        }

        private void Start() {
            foreach (Pool pool in _Pools) {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.Size; i++) {
                    GameObject item = pool.Spawn();
                    item.SetActive(false);
                    item.transform.SetParent(_ObjectParent);
                    objectPool.Enqueue(item);
                }

                _PoolDictionary.Add(pool.Name, objectPool);
            }
        }
        #endregion

        #region PROPERTIES
        public static ObjectPooler Instance {
            get { return __Instance; }
            set { __Instance = value; }
        }
        #endregion

    }
}