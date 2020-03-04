namespace ToughPath.Gameplay {
    using UnityEngine;
    using DG.Tweening;
    using System.Collections;

    public class BridgeManager: MonoBehaviour {

        #region FIELDS
        private static BridgeManager __Instance;

        [SerializeField] private Pool _Pool;

        [SerializeField] private Transform _BridgeParent;

        private int       _BridgeAmount;
        private int       _PlatformsLeft;
        private bool      _BridgePhase;
        private bool      _AllBridgesSpawned;
        private Transform _BridgeStart;
        private Transform _BridgeEnd;

        private const int   _DefaultPlatformAmount  = 3;
        private const float _BridgeSpeed            = -4.0f;
        private const float _DistanceBetweenSpawns  = 2.5f;
        private const float _DistanceToTravel       = 10.0f;
        private const float _TimeToMovePlatform     = 1.0f;
        private const float _CalculationMargin      = 0.1f;
        #endregion

        #region PUBLIC METHODS
        public void StopBridgePhase() {
            if (BridgePhase) {
                _BridgePhase = false;
            }
        }

        public void StartBridgePhase() {
            BridgeAmount = 0;
            AllBridgesSpawned = false;
            var distance = _BridgeEnd.position.x;
            distance += Random.Range(_DistanceToTravel, _DistanceToTravel * Mathf.Max(ScoreManager.Score, 1));
            distance -= Mathf.Abs(_BridgeStart.position.x - distance) % _DistanceBetweenSpawns;
            _BridgePhase = true;

            SetPlatformAmount();

            _BridgeEnd.DOMoveX(distance, _TimeToMovePlatform)
                .OnComplete(() => StartCoroutine(SpawnBridges()));
        }

        public IEnumerator DespawnObjectAfterBridgePhase(string poolName, GameObject objectToDespawn) {
            yield return new WaitWhile(() => BridgePhase);
            ObjectPooler.Instance.EnqueueObject(poolName, objectToDespawn);
        }
        #endregion

        #region PRIVATE/PROTECTED METHODS
        private void SetPlatformAmount() {
            _PlatformsLeft = Random.Range(_DefaultPlatformAmount, (_DefaultPlatformAmount + Mathf.Max(ScoreManager.Score, 1)));
        }

        private void MoveBridge() {
            if (HealthManager.PlayerHealth > 0) {
                if (BridgePhase) {
                    if (AllBridgesSpawned) {
                        BridgeParent.Translate(new Vector3(_BridgeSpeed, 0, 0) * Time.deltaTime);
                    }
                }
            }
        }

        private IEnumerator SpawnBridges() {
            if (BridgeStart != null) {
                if (BridgeEnd != null) {
                    BridgeEnd.GetComponent<Platform>().SetPositionToReach();
                    _BridgeStart.SetParent(BridgeParent);
                    _BridgeEnd.SetParent(BridgeParent);

                    var position = BridgeStart.position;
                    position += new Vector3(_DistanceBetweenSpawns / 2, -_DistanceToTravel);

                    do {
                        var bridge = ObjectPooler.Instance.SpawnFromPool(_Pool.Name, position, Quaternion.identity);
                        bridge.transform.SetParent(BridgeParent);
                        bridge.transform.DOMoveY(bridge.transform.position.y + _DistanceToTravel, _TimeToMovePlatform);

                        StartCoroutine(DespawnObjectAfterBridgePhase(_Pool.Name, bridge));

                        yield return new WaitForSecondsRealtime(0.1f);

                        position += new Vector3(_DistanceBetweenSpawns, 0, 0);
                        BridgeAmount++;
                    } while (position.x + (_DistanceBetweenSpawns / 2) <= BridgeEnd.position.x + _CalculationMargin);

                    ObstacleManager.Instance.SpawnObstacles();

                    yield return new WaitForSecondsRealtime(_TimeToMovePlatform);

                    _AllBridgesSpawned = true;
                }
            }
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
        }

        private void Start() {
            _PlatformsLeft = _DefaultPlatformAmount;
        }

        private void FixedUpdate() {
            MoveBridge();
        }
        #endregion

        #region PROPERTIES
        public static BridgeManager Instance {
            get { return __Instance; }
            set { __Instance = value; }
        }

        public int PlatformsLeft {
            get { return _PlatformsLeft; }
            set { _PlatformsLeft = value; }
        }

        public Transform BridgeStart {
            get { return _BridgeStart; }
            set { _BridgeStart = value; }
        }

        public Transform BridgeEnd {
            get { return _BridgeEnd; }
            set { _BridgeEnd = value; }
        }

        public bool BridgePhase {
            get { return _BridgePhase; }
        }

        public int BridgeAmount {
            get { return _BridgeAmount; }
            set { _BridgeAmount = value; }
        }

        public bool AllBridgesSpawned {
            get { return _AllBridgesSpawned; }
            set { _AllBridgesSpawned = value; }
        }

        public Transform BridgeParent {
            get { return _BridgeParent; }
            set { _BridgeParent = value; }
        }
        #endregion

    }
}