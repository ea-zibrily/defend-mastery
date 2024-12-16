using UnityEngine;
using UnityEngine.UI;
using Defend.Enum;
using Defend.Audio;
using Defend.Rhythm;

namespace Defend.UI
{
    public class MenuManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Music")]
        [SerializeField] private Musics musicName;
        private float fadeDuration;

        [Header("UI")]
        [SerializeField] private Button playButtonUI;
        [SerializeField] private Button tutorialButtonUI;
        [SerializeField] private GameObject tutorialPanelUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            // Init and subs
            tutorialPanelUI.SetActive(false);

            playButtonUI.onClick.AddListener(PlayGame);
            tutorialButtonUI.onClick.AddListener(TutorialGame);

            // Set bgm
            fadeDuration = SceneTransitionManager.Instance.FadeDuration;
            FadeAudio(isFadeIn: true, fadeDuration);
        }

        #endregion

        #region Methods
        
        private void PlayGame()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            FadeAudio(isFadeIn: false, fadeDuration);
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }
        
        private void TutorialGame()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            tutorialPanelUI.SetActive(true);
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
