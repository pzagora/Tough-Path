namespace ToughPath.Gameplay {
    using UnityEngine;
    using DG.Tweening;
    using System.Collections;

    public class PlayerMovement: MonoBehaviour {

        #region FIELDS
        private Animator    _Animator;
        private GameObject  _CurrentPlatform;
        private Rigidbody2D _Rigidbody;
        private Transform   _Transform;
        private Tween       _SizeTween;

        [SerializeField] private Vector2    _Speed;
        [SerializeField] private GameObject _WhiteShield;
        [SerializeField] private GameObject _BlackShield;

        private const string _IsWalking      = "isWalking";
        private const string _IsJumping      = "isJumping";
        private const string _IsFalling      = "isFalling";
        private const float  _NormalSize     = 0.5f;
        private const float  _SmallSize      = 0.2f;
        private const float  _DefenseTime    = 0.55f;
        private const float  _SizeChangeTime = 0.15f;
        private const float  _GroundPosition = -2.3f;
        private const float  _JumpForce      = 400f;
        #endregion

        #region PRIVATE METHODS
        private void SetAnimationState() {
            if (_Rigidbody.velocity.y == 0) {
                _Animator.SetBool(_IsWalking, true);
                _Animator.SetBool(_IsJumping, false);
                _Animator.SetBool(_IsFalling, false);
            } else {
                _Animator.SetBool(_IsWalking, false);
                if (_Rigidbody.velocity.y > 0) {
                    _Animator.SetBool(_IsJumping, true);
                    _Animator.SetBool(_IsFalling, false);
                } else if (_Rigidbody.velocity.y < 0) {
                    _Animator.SetBool(_IsJumping, false);
                    _Animator.SetBool(_IsFalling, true);
                }
            }
        }

        private bool Grounded() {
            if (_Animator.GetBool(_IsWalking) && _Transform.position.y < _GroundPosition) {
                return true;
            } else {
                return false;
            }
        }

        private void ChooseMove() {
            if (InputManager.Instance.ReadyForInputBridge()) {
                if (_Animator.GetBool(_IsWalking)) {
                    if (InputManager.SwipeDirection == Swipe.Up) {
                        DisableShields();
                        _Rigidbody.AddForce(Vector2.up * _JumpForce);
                    } else if(InputManager.SwipeDirection == Swipe.Down) {
                        StartCoroutine(ChangeSize());
                    } else if(InputManager.SwipeDirection == Swipe.UpLeft) {
                        StartCoroutine(EnableShield(false));
                    } else if (InputManager.SwipeDirection == Swipe.DownRight) {
                        StartCoroutine(EnableShield(true));
                    }
                } else {
                    if (InputManager.SwipeDirection == Swipe.Down) {
                        _Rigidbody.AddForce(Vector2.down * _JumpForce);
                    }
                }
            }
        }

        private IEnumerator EnableShield(bool blackShield) {
            if (!_BlackShield.activeInHierarchy && !_WhiteShield.activeInHierarchy) {
                if (blackShield) {
                    _BlackShield.SetActive(true);
                } else {
                    _WhiteShield.SetActive(true);
                }

                yield return new WaitForSecondsRealtime(_DefenseTime);

                DisableShields();
            }
        }

        private void DisableShields() {
            if (_BlackShield.activeInHierarchy) {
                _BlackShield.SetActive(false);
            }
            if (_WhiteShield.activeInHierarchy) {
                _WhiteShield.SetActive(false);
            }
        }

        private IEnumerator ChangeSize() {
            if (_SizeTween == null || !_SizeTween.IsActive()) {
                _SizeTween = _Transform.DOScale(new Vector3(_SmallSize, _SmallSize, 1), _SizeChangeTime);

                yield return new WaitForSecondsRealtime(_DefenseTime);

                _SizeTween = _Transform.DOScale(new Vector3(_NormalSize, _NormalSize, 1), _SizeChangeTime);
            }
        }
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            _Rigidbody = GetComponent<Rigidbody2D>();
            _Animator = GetComponent<Animator>();
            _Transform = gameObject.transform;
        }

        private void Update() {
            SetAnimationState();
        }

        private void FixedUpdate() {
            ChooseMove();
        }

        // Land on platform
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "Platform") {
                BridgeManager.Instance.StopBridgePhase();

                _Rigidbody.velocity = Vector3.zero;

                var platform = collision.gameObject.GetComponent<Platform>();
                platform.SetPositionToReach();
                var collisionTransform = collision.transform;

                if (!(BridgeManager.Instance.BridgePhase && platform.FirstTouch)) {
                    CameraManager.Instance.MoveCameraJumpPhase(collisionTransform.position);
                    PlatformManager.Instance.CurrentPlatform = collisionTransform;
                    JumpManager.Instance.MoveToPosition(platform.PositionToReach.x, platform.FirstTouch);
                }

                if (platform.FirstTouch) {
                    if (ScoreManager.Score >= 0) {
                        AudioManager.Instance.Play("ReachPlatform", true);
                    }
                    ScoreManager.AddScore(1);
                    platform.FirstTouch = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.tag == "Kill") {
                HealthManager.ReduceHealth(HealthManager.MaxHealth);
            }
        }
        #endregion

    }
}