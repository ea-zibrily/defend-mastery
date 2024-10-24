using UnityEngine;
using UnityEngine.UI;
using Defend.Enum;
using Defend.Audio;
using Defend.Managers;

namespace Defend.UI
{
    public class MenuManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")]
        [SerializeField] private Button playButtonUI;
        [SerializeField] private Button tutorialButtonUI;
        [SerializeField] private GameObject tutorialPanelUI;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            tutorialPanelUI.SetActive(false);

            playButtonUI.onClick.AddListener(PlayGame);
            tutorialButtonUI.onClick.AddListener(TutorialGame);
        }

        #endregion

        #region Methods
        
        private void PlayGame()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.NextLevel);
        }

        private void TutorialGame()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            tutorialPanelUI.SetActive(true);
        }

        #endregion
    }
}
