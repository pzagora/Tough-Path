namespace ToughPath.Gameplay {
    using UnityEngine;
    using DG.Tweening;

    public class Shield: MonoBehaviour {

        private SpriteRenderer _SpriteRenderer;

        private const float _TimeToFade = 0.35f;

        private void Awake() {
            _SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable() {
            Color color = _SpriteRenderer.color;
            color.a = 0;
            _SpriteRenderer.color = color;

            _SpriteRenderer.DOFade(1, _TimeToFade);
        }

    }
}