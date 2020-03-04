namespace ToughPath.Gameplay {
    using UnityEngine;
    using DG.Tweening;

    public class Projectile: MonoBehaviour {

        #region FIELDS
        [SerializeField] private Pool _Pool;

        private GameObject  _GameObject;
        private Transform   _Transform;
        private bool        _Triggered;

        private const float _MinOffsetX       = 3.0f;
        private const float _MaxOffsetX       = 10.0f;
        private const float _TargetRandomness = 2.5f;
        private const float _MinTimeToHit     = 0.85f;
        private const float _MaxTimeToHit     = 1.35f;
        private const float _OffsetY          = -3.0f;
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            _GameObject = gameObject;
            _Transform = gameObject.transform;
        }

        private void OnEnable() {
            _Triggered = false;
        }

        private void EnqueueProjectile() { }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag != "Player") {
                AudioManager.Instance.Play("BombHit", true);
            }
            ObjectPooler.Instance.EnqueueObject(_Pool.Name, _GameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (!_Triggered) {
                if (collision.gameObject.tag == "Player") {
                    var target = new Vector3(collision.transform.position.x + Random.Range(-_TargetRandomness, 1), _OffsetY, 0);
                    _Transform.position += new Vector3(Random.Range(_MinOffsetX, _MaxOffsetX), 0);
                    _Transform.DOMove(target, Random.Range(_MinTimeToHit, _MaxTimeToHit))
                        .SetEase(Ease.Linear)
                        .OnComplete(() => ObjectPooler.Instance.EnqueueObject(_Pool.Name, _GameObject));
                }
                _Triggered = true;
            }
        }
        #endregion

    }
}


