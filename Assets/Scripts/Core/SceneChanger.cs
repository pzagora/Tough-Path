namespace ToughPath.Core {
    using UnityEngine.SceneManagement;
    using DG.Tweening;
    using UnityEngine.UI;

    public static class SceneChanger {

        #region PUBLIC METHODS
        public static Tween ChangeScene(Image image, float amount, float time, string scene) {
            return image.DOFade(amount, time).OnComplete(() => SceneManager.LoadSceneAsync(scene));
        }

        public static string SceneName() {
            return SceneManager.GetActiveScene().name;
        }
        #endregion
    }
}