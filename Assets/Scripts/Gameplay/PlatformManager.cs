namespace ToughPath.Gameplay {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    public class PlatformManager: MonoBehaviour {

        #region FIELDS
        private static PlatformManager __Instance;

        [SerializeField] private GameObject _SpawnPoint;
        [SerializeField] private Transform  _PlatformParent;
        [SerializeField] private Vector3    _DefaultPosition;

        private Transform  _CurrentPlatform;
        private bool       _FirstPlatform;
        private float      _PlatformMaxDistanceOnScreen;

        private const float _PlatformMaxDistance = 8.2f;
        private const float _PlatformMinDistance = 3.75f;
        private const float _PlatformPositionY   = -7f;
        private const float _MoveDistanceY       = 3.5f;
        private const float _MoveDuration        = 1.0f;
        #endregion

        #region PUBLIC METHODS
        public void EnableNextPlatform() {
            Vector3 platformPosition;

            if (_FirstPlatform) {
                platformPosition = new Vector3(_DefaultPosition.x, _PlatformPositionY);
            } else {
                platformPosition = new Vector3(CalculateNextPlatformDistance(), _PlatformPositionY);
            }

            var platform = ObjectPooler.Instance.SpawnFromPool("Platform", platformPosition, Quaternion.identity);
            platform.gameObject.transform.SetParent(PlatformParent);
            BridgeManager.Instance.BridgeStart = CurrentPlatform;
            BridgeManager.Instance.BridgeEnd = platform.transform;

            MovePlatformUp(platform.transform);
        }
        #endregion

        #region PRIVATE/PROTECTED METODS
        private void MovePlatformUp(Transform platform) {
            platform.DOMoveY(platform.position.y + _MoveDistanceY, _MoveDuration);
        }

        private float CalculateNextPlatformDistance() {
            return Random.Range(CurrentPlatform.position.x + _PlatformMinDistance,
                CurrentPlatform.position.x + _PlatformMaxDistanceOnScreen);
        }
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }

            _DefaultPosition = _SpawnPoint.transform.position;
            _FirstPlatform = true;
        }

        private void Start() {
            EnableNextPlatform();
            _FirstPlatform = false;

            _PlatformMaxDistanceOnScreen = _PlatformMaxDistance * CameraManager.Instance.AspectRatio;
        }
        #endregion

        #region PROPERTIES
        public Transform PlatformParent {
            get { return _PlatformParent; }
        }

        public static PlatformManager Instance {
            get { return __Instance; }
            set { __Instance = value; }
        }

        public Transform CurrentPlatform {
            get { return _CurrentPlatform; }

            set { _CurrentPlatform = value; }
        }
        #endregion

    }
}