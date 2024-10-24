using System;
using UnityEngine;
using Defend.Enum;
using Defend.Managers;

namespace Defend.Audio
{
    public class AudioController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private Musics musicName;
        
        // Event
        public static event Action<bool, float> OnFadeAudio;

        #endregion
    
        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            OnFadeAudio += FadeAudio;
        }

        private void OnDisable()
        {
            OnFadeAudio -= FadeAudio;
        }
        
        private void Start()
        {
            var fadeDuration = SceneTransitionManager.Instance.FadeDuration;
            FadeAudioEvent(isFadeIn: true, fadeDuration);
        }
        
        #endregion

        #region Methods

        public static void FadeAudioEvent(bool isFadeIn, float duration = 0.5f)
        {
            OnFadeAudio?.Invoke(isFadeIn, duration);
        }
        
        private void FadeAudio(bool isFadeIn, float duration)
        {
            var audio = AudioManager.Instance.GetAudio(musicName);
            var volume = audio.volume;
            
            if (isFadeIn)
                StartCoroutine(AudioHelpers.FadeIn(audio.source, duration, volume));
            else
                StartCoroutine(AudioHelpers.FadeOut(audio.source, duration));
        }

        #endregion
    }
}
