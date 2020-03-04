namespace ToughPath.Gameplay {
    using UnityEngine;

    public static class ScoreManager {

        #region FIELDS
        private static int  _Score;
        private static bool _NewHighscore;
        #endregion

        #region PUBLIC METHODS
        public static void AddScore(int score) {
            _Score += score;

            if (_Score > PlayerPrefs.GetInt("Highscore", 0)) {
                _NewHighscore = true;
                PlayerPrefs.SetInt("Highscore", _Score);
            }

            UI.InGameCanvasManager.Instance.UpdateScore();
        }

        public static void ResetScore() {
            _Score = -1;
            _NewHighscore = false;
        }
        #endregion

        #region PROPERTIES
        public static int Score {
            get { return _Score; }
        }

        public static bool NewHighscore {
            get { return _NewHighscore; }
        }
        #endregion

    }
}