namespace ToughPath.UI {
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;

    [RequireComponent(typeof(Button))]
    public class ExitGame: MonoBehaviour {

        #region PUBLIC METHODS
        public void CloseGame() {
            Application.Quit();
        }
        #endregion
    }
}
