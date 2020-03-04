namespace ToughPath.Gameplay {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    [RequireComponent(typeof(Camera))]
    public class CameraManager: MonoBehaviour {

        #region FIELDS
        private static CameraManager __Instance;

        private float     _AspectRatio;
        private Transform _Transform;

        private const float _CameraOffset  = 4.5f;
        private const float _TimeToMove    = 2.0f;
        private const float _ShakeDuration = 1.0f;
        #endregion

        #region PUBLIC METHODS
        public void MoveCameraJumpPhase(Vector3 position) {
            _Transform.DOMoveX(position.x + (_CameraOffset * _AspectRatio), _TimeToMove);
        }

        public void ShakeCamera() {
            _Transform.DOShakePosition(_ShakeDuration);
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

            _Transform = gameObject.transform;

            _AspectRatio = GetComponent<Camera>().aspect;
        }
        #endregion

        #region PROPERTIES
        public static CameraManager Instance {
            get { return __Instance; }
            set { __Instance = value; }
        }

        public float AspectRatio {
            get { return _AspectRatio; }
        }
        #endregion

    }
}