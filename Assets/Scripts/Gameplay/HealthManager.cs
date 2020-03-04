namespace ToughPath.Gameplay {
    using UnityEngine;

    public class HealthManager: MonoBehaviour {

        #region FIELDS
        private static int _PlayerHealth;

        private const int _MaxHealth = 3;
        #endregion

        #region PUBLIC METHODS
        public static void ReduceHealth(int damage) {
            _PlayerHealth -= damage;
            _PlayerHealth = Mathf.Clamp(_PlayerHealth, 0, MaxHealth);

            if (damage < MaxHealth) {
                CameraManager.Instance.ShakeCamera();
                AudioManager.Instance.Play("Hit", true);
            } else {
                AudioManager.Instance.Play("Fall", false);
            }

            UI.InGameCanvasManager.Instance.UpdateHealth(_PlayerHealth, MaxHealth);
        }
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Start() {
            _PlayerHealth = MaxHealth;
        }
        #endregion

        #region PROPERTIES
        public static int PlayerHealth {
            get { return _PlayerHealth; }
        }

        public static int MaxHealth {
            get { return _MaxHealth; }
        }
        #endregion

    }
}