namespace ToughPath.Gameplay {
    using System.Collections;
    using DG.Tweening;
    using UnityEngine;

    public class Platform: MonoBehaviour {

        #region FIELDS
        [SerializeField] private Pool _Pool;

        private Coroutine  _Coroutine;
        private GameObject _GameObject;
        private Transform  _Transform;
        private Vector3    _PositionToReach;
        private bool       _FirstTouch;

        private const float _MaxWidth       = 2.0f;
        private const float _MinWidth       = 0.75f;
        private const float _MoveDistanceY  = -2.5f;
        private const float _TimeToDisable  = 4.0f;
        private const float _TimeToMove     = 1.0f;
        #endregion

        #region PUBLIC METHODS
        public void SetPositionToReach() {
            PositionToReach = _Transform.GetChild(0).transform.position;
        }
        #endregion

        #region PRIVATE/PROTECTED METHODS
        private IEnumerator DisablePlatform() {
            yield return new WaitForSecondsRealtime(_TimeToDisable);
            _Transform.DOMoveY(_Transform.position.y + _MoveDistanceY, _TimeToMove)
                .OnComplete(() => ObjectPooler.Instance.EnqueueObject(_Pool.Name, gameObject));
        }
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            _GameObject = gameObject;
            _Transform = _GameObject.transform;
        }

        private void OnEnable() {
            Vector3 size = _Transform.localScale;
            size.x = Random.Range(_MinWidth, _MaxWidth);
            _Transform.localScale = size;
            _FirstTouch = true;
        }

        private void OnCollisionExit2D(Collision2D collision) {
            if (collision.gameObject.tag == "Player") {
                _Coroutine = StartCoroutine(DisablePlatform());
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (_Coroutine != null) {
                StopCoroutine(_Coroutine);
                _Coroutine = null;
            }
        }
        #endregion

        #region PROPERTIES
        public Vector3 PositionToReach {
            get { return _PositionToReach; }
            set { _PositionToReach = value; }
        }

        public bool FirstTouch {
            get { return _FirstTouch; }
            set { _FirstTouch = value; }
        }
        #endregion
    }
}