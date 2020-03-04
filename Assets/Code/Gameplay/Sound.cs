namespace ToughPath.Gameplay {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Audio;

    [CreateAssetMenu(fileName = "Sound", menuName = "Create Sound")]
    public class Sound: ScriptableObject {

        #region FIELDS
        [HideInInspector] public AudioSource Source;

        public string    Name;
        public AudioClip Clip;
        
        [Range(0f, 1f)]
        public float Volume;

        public float MinPitchValue = 0.85f;
        public float MaxPitchValue = 1.15f;
        #endregion

    }
}