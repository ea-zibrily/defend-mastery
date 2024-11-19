using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Defend.Enum;
using Defend.Audio;
using Defend.Singleton;
using DG.Tweening;

namespace Defend.Managers
{
    public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
    {
        #region Fields & Properties
    
        [Header("Scene-Fader")]
        [SerializeField] [Range(0f, 3f)] private float fadeDuration = 0.5f;
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        
        private Tween _fadeTween;
        public float FadeDuration => fadeDuration;
        
        // Cached Scene Name
        private const string MAIN_MENU = "main-menu";
        private const string OLD_TRAFFORD = "old-trafford";

        #endregion
        
        #region MonoBehaviour Callbacks
    
        private void OnEnable()
        {
            DOTween.Init(true, false, LogBehaviour.Verbose).SetCapacity(900, 360);
        }
        
        private void Start()
        {
            FadeIn();
        }
        
        #endregion

        #region Fader

        private void FadeIn()
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            
            DoFade(1f, 0f);
            DoFade(0f, fadeDuration, () => 
            {
                fadeCanvasGroup.interactable = true;
                fadeCanvasGroup.gameObject.SetActive(false);
            });
        }
        
        private void FadeOut()
        {
            fadeCanvasGroup.gameObject.SetActive(true);

            DoFade(0f, 0f);
            DoFade(1f, fadeDuration, () =>
            {
                fadeCanvasGroup.interactable = false;
            });
        }
        
        private void DoFade(float target, float duration, TweenCallback callback = null)
        {
            _fadeTween?.Kill(false);
            _fadeTween = fadeCanvasGroup.DOFade(target, duration);
            _fadeTween.onComplete += callback;
        }

        #endregion

        #region Methods

        // !- Core
        public void LoadSelectedScene(SceneState sceneState)
        {
            Time.timeScale = 1;
            DOTween.KillAll(false);
            
            switch (sceneState)
            {
                case SceneState.MainMenu:
                    FadeAndLoadScene(() => LoadMainMenu());
                    break;
                case SceneState.CurrentLevel:
                    FadeAndLoadScene(() => LoadCurrentLevel());
                    break;
                case SceneState.NextLevel:
                    FadeAndLoadScene(() => LoadNextLevel());
                    break;
            }
        }
        
        private void FadeAndLoadScene(Action loadSceneAction)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            AudioController.FadeAudioEvent(isFadeIn: false, FadeDuration);

            DoFade(0, 0);
            DoFade(1, FadeDuration, () =>
            {
                Invoke(loadSceneAction.Method.Name, FadeDuration);
            });
        }
        
        private void LoadMainMenu() => SceneManager.LoadScene(MAIN_MENU);
        private void LoadCurrentLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        private void LoadNextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        #endregion

    }
}
