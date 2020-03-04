namespace ToughPath.Gameplay {
    using UnityEngine;

    public class Damager: MonoBehaviour {

        #region FIELDS
        [SerializeField] private int  _Damage;
        [SerializeField] private bool _HaveParent;
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "Player") {
                HealthManager.ReduceHealth(_Damage);
                if (_HaveParent) {
                    transform.parent.gameObject.SetActive(false);
                } else {
                    gameObject.SetActive(false);
                }
            }
        }
        #endregion

    }
}
