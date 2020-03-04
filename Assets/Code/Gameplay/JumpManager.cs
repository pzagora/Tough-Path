namespace ToughPath.Gameplay {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    public class JumpManager: MonoBehaviour {

        #region FIELDS
        private static JumpManager __Instance;

        [SerializeField] private Vector2 _JumpAngle;
        [SerializeField] private float   _JumpForce;
        [SerializeField] private bool    _JumpPhase;
        [SerializeField] private bool    _PickingAngle;
        [SerializeField] private bool    _Flag;
        
        private Rigidbody2D _PlayerRigidbody;

        private const float _ForceChargeSpeed = 800.0f;
        private const float _AngleChangeSpeed = 75.0f;
        private const float _MaxJumpAngle     = 80.0f;
        private const float _MinJumpAngle     = 10.0f;
        private const float _MaxJumpForce     = 1000.0f;
        private const float _MinJumpForce     = 200.0f;
        private const float _WalkDuration     = 2.0f;
        #endregion

        #region PUBLIC METHODS
        public void MoveToPosition(float finalPositionOnX, bool spawnPlatform) {
            if (spawnPlatform) {
                PlatformManager.Instance.EnableNextPlatform();
            }
            _PlayerRigidbody.DOMoveX(finalPositionOnX, _WalkDuration).SetEase(Ease.InOutExpo).OnComplete(() => ChoosePhase(spawnPlatform));
        }
        #endregion

        #region PRIVATE/PROTECTED METHODS
        private void ChoosePhase(bool newPlatform) {
            if (newPlatform) {
                if (BridgeManager.Instance.PlatformsLeft <= 0) {
                    BridgeManager.Instance.StartBridgePhase();
                } else {
                    BridgeManager.Instance.PlatformsLeft--;
                    StartJumpPhase();
                }
            } else {
                if (!BridgeManager.Instance.BridgePhase) {
                    StartJumpPhase();
                }
            }
        }

        private void StartJumpPhase() {
            _JumpForce = _MinJumpForce;
            _PickingAngle = true;
            JumpPhase = true;
        }

        private void CalculateJump() {
            if (JumpPhase) {
                if (_PickingAngle) {
                    RotateJumpArrow(UI.InGameCanvasManager.Instance.PlayerCanvasTransform);
                    _JumpAngle = CalculateVectorDistance(UI.InGameCanvasManager.Instance.PlayerCanvasTransform.position, UI.InGameCanvasManager.Instance.JumpArrowTransform.position);
                }
                if (InputManager.JumpPhaseState == JumpState.Angle) {
                    _JumpForce = _MinJumpForce;
                    _PickingAngle = false;
                } else if (InputManager.JumpPhaseState == JumpState.Force && !_PickingAngle) {
                    OscilateBetweenValues(ref _JumpForce, _MinJumpForce, _MaxJumpForce, _ForceChargeSpeed);
                } else if (InputManager.JumpPhaseState == JumpState.Jump && !_PickingAngle) {
                    Jump(_JumpAngle, _JumpForce);
                }
            }
        }

        private void OscilateBetweenValues(ref float variableToChange, float minValue, float maxValue, float changeAmount) {
            if (!_Flag) {
                variableToChange += changeAmount * Time.deltaTime;
                if (variableToChange >= maxValue) {
                    variableToChange = Mathf.Clamp(variableToChange, minValue, maxValue);
                    _Flag = true;
                }
            } else {
                variableToChange -= changeAmount * Time.deltaTime;
                if (variableToChange <= minValue) {
                    variableToChange = Mathf.Clamp(variableToChange, minValue, maxValue);
                    _Flag = false;
                }
            }
        }

        private void Jump(Vector2 angle, float force) {
            _PlayerRigidbody.AddForce(angle.normalized * force);
            AudioManager.Instance.Play("Jump", true);
            JumpPhase = false;
        }

        private void RotateJumpArrow(Transform jumpArrowCanvas) {
            if (!_Flag) {
                jumpArrowCanvas.Rotate(Vector3.forward * _AngleChangeSpeed * Time.deltaTime);
                if (jumpArrowCanvas.eulerAngles.z >= _MaxJumpAngle) {
                    _Flag = true;
                }
            } else {
                jumpArrowCanvas.Rotate(Vector3.forward * -_AngleChangeSpeed * Time.deltaTime);
                if (jumpArrowCanvas.eulerAngles.z <= _MinJumpAngle) {
                    _Flag = false;
                }
            }
        }

        private Vector2 CalculateVectorDistance(Vector3 firstVector, Vector3 secondVector) {
            return -(new Vector2((firstVector.x - secondVector.x), (firstVector.y - secondVector.y)));
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
            _PlayerRigidbody = InputManager.Instance.PlayerRigidbody();
            _JumpForce = _MinJumpForce;
            UI.InGameCanvasManager.Instance.PlayerCanvasTransform.eulerAngles = Vector3.forward * _MinJumpAngle;
        }

        private void Update() {
            CalculateJump();
        }
        #endregion

        #region PROPERTIES
        public bool JumpPhase {
            get { return _JumpPhase; }
            set { _JumpPhase = value; }
        }

        public static JumpManager Instance {
            get { return __Instance; }
            set { __Instance = value; }
        }

        public float JumpForce {
            get { return _JumpForce; }
        }
        #endregion

    }
}
