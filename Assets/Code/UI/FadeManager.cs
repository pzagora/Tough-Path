namespace ToughPath.UI {
    using UnityEngine.UI;
    using UnityEngine;
    using DG.Tweening;

    public class FadeManager: MonoBehaviour {

        #region FIELDS
        [SerializeField] private ChangeSceneProperties _Properties;
        [SerializeField] private Transform             _Canvas;

        private Image _ScreenFade;
        private Tween _FadeTween;
        private bool  _SceneIsLoading;
        #endregion

        #region PUBLIC METHODS
        public void StartGame() {
            _FadeTween = Core.SceneChanger.ChangeScene(_ScreenFade, _Properties._FadeOutTarget, _Properties._TimeToFade, _Properties._TargetScene);
            PlayButtonPressSound();
        }

        public void RestartGame() {
            _FadeTween = Core.SceneChanger.ChangeScene(_ScreenFade, _Properties._FadeOutTarget, _Properties._TimeToFade, "Game");
            PlayButtonPressSound();
        }
        #endregion

        #region PRIVATE/PROTECTED METHODS
        private void PlayButtonPressSound() {
            Gameplay.AudioManager.Instance.Play("ButtonPress", false);
        }
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            _ScreenFade = Instantiate(_Properties._ScreenFade, _Canvas);
        }

        private void Start() {
            if (!_ScreenFade.gameObject.activeInHierarchy) {
                _ScreenFade.gameObject.SetActive(true);
            }

            _SceneIsLoading = false;
            _FadeTween = _ScreenFade.DOFade(_Properties._FadeInTarget, _Properties._TimeToFade);
        }

        private void Update() {
            if (Core.SceneChanger.SceneName() == "Game") {
                if (Gameplay.HealthManager.PlayerHealth <= 0) {
                    if (!_FadeTween.IsActive() && !_SceneIsLoading) {
                        _SceneIsLoading = true;
                        _FadeTween = Core.SceneChanger.ChangeScene(_ScreenFade, _Properties._FadeOutTarget, _Properties._TimeToFade, _Properties._TargetScene);
                    }
                }
            }
        }
        #endregion

    }
}