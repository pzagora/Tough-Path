namespace ToughPath.UI {
    using UnityEngine;
    using UnityEngine.UI;

    public class GameOver: MonoBehaviour {

        #region FIELDS
        [SerializeField] private GameObject _NewRecord;
        [SerializeField] private Text       _Score;
        [SerializeField] private Text       _Highscore;
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            _NewRecord.SetActive(false);
        }

        private void Start() {
            _Score.text = Gameplay.ScoreManager.Score.ToString();
            _Highscore.text = PlayerPrefs.GetInt("Highscore", 0).ToString();

            if (Gameplay.ScoreManager.NewHighscore) {
                Gameplay.AudioManager.Instance.Play("NewHighscore", false);
                _NewRecord.SetActive(true);
            }
        }
        #endregion

    }
}