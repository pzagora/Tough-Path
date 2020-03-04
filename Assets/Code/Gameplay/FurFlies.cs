namespace ToughPath.Gameplay {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FurFlies: MonoBehaviour {

        #region FIELDS
        private List<Transform> _Furflies;

        private const float _MinOffsetY    = 1.5f;
        private const float _MaxOffsetY    = 3.0f;
        private const float _MaxSpawnRange = 3.0f;
        private const float _MinScale      = 0.35f;
        private const float _MaxScale      = 1.5f;
        #endregion

        #region IMPLEMETETION OF: MonoBehaviour
        private void Awake() {
            _Furflies = new List<Transform>();

            foreach (Transform child in transform) {
                _Furflies.Add(child);
            }
        }

        private void OnEnable() {
            float offsetY = _MaxOffsetY;
            foreach (Transform item in _Furflies) {
                if (item != _Furflies[0]) {
                    Vector3 newPosition = new Vector3(Random.Range(-_MaxSpawnRange, _MaxSpawnRange), offsetY, 0);
                    item.localPosition = newPosition;
                    float scale = Random.Range(_MinScale, _MaxScale);
                    item.localScale = new Vector3(scale, scale, 1);
                    offsetY += Random.Range(_MinOffsetY, _MaxOffsetY);
                }
            }
        }
        #endregion

    }
}