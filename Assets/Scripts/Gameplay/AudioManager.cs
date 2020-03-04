namespace ToughPath.Gameplay {
    using System;
    using UnityEngine;

    public class AudioManager: MonoBehaviour {

        #region FIELDS
        private static AudioManager __Instance;

        private GameObject _GameObject;
        [SerializeField] private Sound[] _Sounds;
        #endregion

        #region PUBLIC METHODS
        public void Play(string soundName, bool randomPitch) {
            Sound sound = Array.Find(_Sounds, item => item.name == soundName);
            if (randomPitch) {
                sound.Source.pitch = UnityEngine.Random.Range(sound.MinPitchValue, sound.MaxPitchValue);
            }
            sound.Source.Play();
        }
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                return;
            }

            _GameObject = gameObject;
            DontDestroyOnLoad(_GameObject);

            foreach (Sound sound in _Sounds) {
                sound.Source = _GameObject.AddComponent<AudioSource>();
                sound.Source.clip = sound.Clip;
                sound.Source.volume = sound.Volume;
            }
        }
        #endregion

        #region PROPERTIES
        public static AudioManager Instance {
            get { return __Instance; }
            set { __Instance = value; }
        }
        #endregion

    }
}