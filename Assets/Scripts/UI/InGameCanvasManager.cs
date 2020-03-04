namespace ToughPath.UI {
    using UnityEngine.UI;
    using UnityEngine;
    using DG.Tweening;

    public class InGameCanvasManager: MonoBehaviour {

        #region FIELDS
        private static InGameCanvasManager __Instance;

        [SerializeField] private Image      _Health;
        [SerializeField] private Transform  _JumpArrowTransform;
        [SerializeField] private Transform  _PlayerCanvasTransform;
        [SerializeField] private Slider     _PowerSlider;
        [SerializeField] private Text       _Score;

        private GameObject _JumpArrowCanvas;

        private const float _TimeToChangeFill = 1.0f;
        #endregion

        #region PUBLIC METHODS
        public void UpdateScore() {
            _Score.text = Gameplay.ScoreManager.Score.ToString();
        }

        public void UpdateHealth(float currentHealth, float maxHealth) {
            float fillTarget;

            if (currentHealth == 0) {
                fillTarget = currentHealth;
            } else {
                fillTarget = ((1 / maxHealth) * currentHealth);
            }

            _Health.DOFillAmount(fillTarget, _TimeToChangeFill);
        }
        #endregion

        #region PRIVATE/PROTECTED METHODS
        private void ToggleJumpArrow() {
            if (Gameplay.JumpManager.Instance.JumpPhase) {
                _PowerSlider.value = Gameplay.JumpManager.Instance.JumpForce;
                if (!_JumpArrowCanvas.activeInHierarchy) {
                    _JumpArrowCanvas.SetActive(true);
                }
            } else {
                if (_JumpArrowCanvas.activeInHierarchy) {
                    _JumpArrowCanvas.SetActive(false);
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

            _JumpArrowCanvas = _PlayerCanvasTransform.gameObject;
        }

        private void Start() {
            Gameplay.ScoreManager.ResetScore();
        }

        private void Update() {
            ToggleJumpArrow();
        }
        #endregion

        #region PROPERTIES
        public Transform PlayerCanvasTransform {
            get { return _PlayerCanvasTransform; }
        }

        public Transform JumpArrowTransform {
            get { return _JumpArrowTransform; }
        }

        public static InGameCanvasManager Instance {
            get { return __Instance; }
            set { __Instance = value; }
        }
        #endregion

    }
}