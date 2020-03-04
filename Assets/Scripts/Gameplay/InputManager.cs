namespace ToughPath.Gameplay {
    using UnityEngine;
    using DG.Tweening;
    
    public enum Swipe { None, Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight };
    public enum JumpState { None, Angle, Force, Jump};

    public class InputManager: MonoBehaviour {
        
        #region FIELDS
        private static InputManager __Instance;
        
        private static Vector2 __CurrentSwipe;
        private static Vector2 __PressPosition;
        private static Vector2 __ReleasePosition;
        private static Swipe   __SwipeDirection;
        private static JumpState __JumpPhaseState;

        private const float _MinSwipeLength  = 200f;
        private const float _SwipeDetectArea = 0.5f;
        #endregion

        #region PUBLIC METHODS
        public bool ReadyForInputBridge() {
            if (BridgeManager.Instance.BridgePhase) {
                if (BridgeManager.Instance.AllBridgesSpawned) {
                    if (PlayerIsAlive()) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ReadyForInputJump() {
            if (JumpManager.Instance.JumpPhase) {
                if (PlayerIsAlive()) {
                    return true;
                }
            }
            return false;
        }

        public Rigidbody2D PlayerRigidbody() {
            return gameObject.GetComponent<Rigidbody2D>();
        }
        #endregion

        #region PRIVATE/PROTECTED METHODS
        private void GetInputBridge() {
            if (Input.GetKey(KeyCode.UpArrow)) {
                SwipeDirection = Swipe.Up;
            } else if (Input.GetKey(KeyCode.DownArrow)) {
                SwipeDirection = Swipe.Down;
            } else if (Input.GetKey(KeyCode.LeftArrow)) {
                SwipeDirection = Swipe.UpLeft;
            } else if (Input.GetKey(KeyCode.RightArrow)) {
                SwipeDirection = Swipe.DownRight;
            } else {
                SwipeDirection = Swipe.None;
            }
        }

        private void GetInputJump() {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && JumpPhaseState == JumpState.None) {
                JumpPhaseState = JumpState.Angle;
            } else if (JumpPhaseState == JumpState.Angle) {
                JumpPhaseState = JumpState.Force;
            } else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && JumpPhaseState == JumpState.Force) {
                JumpPhaseState = JumpState.Jump;
            } else if (JumpPhaseState == JumpState.Jump) {
                JumpPhaseState = JumpState.None;
            }
        }

        private bool PlayerIsAlive() {
            if (HealthManager.PlayerHealth > 0) {
                return true;
            } else {
                return false;
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

        private void Update() {
            if (ReadyForInputBridge()) {
                GetInputBridge();
            }
            if (ReadyForInputJump()) {
                GetInputJump();
            }
        }
        #endregion

        #region PROPERTIES
        public static InputManager Instance {
            get { return __Instance; }
            set { __Instance = value; }
        }

        public static Swipe SwipeDirection {
            get { return __SwipeDirection; }
            set { __SwipeDirection = value; }
        }

        public static JumpState JumpPhaseState {
            get { return __JumpPhaseState; }
            set { __JumpPhaseState = value; }
        }
        #endregion

    }
}