namespace ToughPath.UI {
    using UnityEngine;
    using UnityEngine.UI;
    
    [CreateAssetMenu(fileName = "ChangeSceneProperties", menuName = "ChangeSceneData")]
    public class ChangeSceneProperties: ScriptableObject {

        #region FIELDS
        [SerializeField] public Image  _ScreenFade;
        [SerializeField] public string _TargetScene;
        [SerializeField] public float  _FadeInTarget  = 0f;
        [SerializeField] public float  _FadeOutTarget = 1.2f;
        [SerializeField] public float  _TimeToFade    = 1.5f;
        #endregion

    }
}