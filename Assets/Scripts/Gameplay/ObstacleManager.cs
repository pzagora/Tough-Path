namespace ToughPath.Gameplay {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ObstacleManager: MonoBehaviour {

        #region FIELDS
        private static ObstacleManager __Instance;

        [SerializeField] private Pool[] _Pools;

        private const int   _MaxSpikesAmount             = 4;
        private const int   _MaxBombsAmount              = 7;
        private const float _Buffer                      = 4.5f;
        private const float _SpawnOnHeight               = 10.0f;
        private const float _SpikesOffsetDistance        = 1.2f;
        private const float _MinBombsOffsetDistance      = -0.15f;
        private const float _MaxBombsOffsetDistance      = 0.35f;
        private const float _MinDistanceBetweenObstacles = 3.2f;
        private const float _MaxDistanceBetweenObstacles = 3.8f;
        #endregion

        #region PUBLIC METHODS
        public void SpawnObstacles() {
            if (BridgeManager.Instance.BridgeStart != null) {
                if (BridgeManager.Instance.BridgeEnd != null) {
                    var position = BridgeManager.Instance.BridgeStart.position + new Vector3(_Buffer, _SpawnOnHeight);

                    do {
                        EnableObstacle(ref position);
                        position.x += Random.Range(_MinDistanceBetweenObstacles, _MaxDistanceBetweenObstacles);
                    } while (position.x <= (BridgeManager.Instance.BridgeEnd.position.x - (_Buffer)));
                }

            }
        }
        #endregion

        #region PRIVATE/PROTECTED METHODS
        private void EnableObstacle(ref Vector3 position) {
            string obstacleName = _Pools[Random.Range(0, _Pools.Length)].Name;
            var amount = 1;

            if (obstacleName == "Spikes") {
                amount = Random.Range(0, _MaxSpikesAmount);
            } else if (obstacleName == "WhiteBombs" || obstacleName == "BlackBombs") {
                amount = Random.Range(0, _MaxBombsAmount);
            }

            for (int i = 0; i < amount; i++) {
                GameObject obstacle;
                if (obstacleName == "WhiteBombs" || obstacleName == "BlackBombs") {
                    obstacle = ObjectPooler.Instance.SpawnFromPool(obstacleName, position + new Vector3(Random.Range(_MinBombsOffsetDistance, _MaxBombsOffsetDistance), 0, 0), Quaternion.identity);
                } else {
                    obstacle = ObjectPooler.Instance.SpawnFromPool(obstacleName, position, Quaternion.identity);
                }
                if (obstacle != null) {
                    obstacle.transform.SetParent(BridgeManager.Instance.BridgeParent);

                    StartCoroutine(BridgeManager.Instance.DespawnObjectAfterBridgePhase(obstacleName, obstacle));

                    if (obstacleName == "Spikes")
                        position.x += _SpikesOffsetDistance;
                }
            }
        }
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }
        #endregion

        #region PROPERTIES
        public static ObstacleManager Instance {
            get { return __Instance; }
            set { __Instance = value; }
        }
        #endregion

    }
}