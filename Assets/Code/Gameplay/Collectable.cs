namespace ToughPath.Gameplay {
    using UnityEngine;

    public class Collectable: MonoBehaviour {

        #region FIELDS
        [SerializeField] private GameObject  _GameObject;
        [SerializeField] private Pool        _Pool;
        [SerializeField] private int         _Score;
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            _GameObject = gameObject;
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.tag == "Player") {
                ObjectPooler.Instance.EnqueueObject(_Pool.Name, _GameObject);
                ScoreManager.AddScore(_Score);
            }
        }
        #endregion

    }
}


