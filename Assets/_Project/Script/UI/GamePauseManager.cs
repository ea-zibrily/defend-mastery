using UnityEngine;
using UnityEngine.UI;
using Defend.Audio;
using Defend.Enum;
using Defend.Managers;

namespace Defend.UI
{
    public class GamePauseManager : GameUI
    {
        #region Fields & Properties

        // UI
        [SerializeField] private Button pauseButtonUI;
        [SerializeField] private Button resumeButtonUI;
        [SerializeField] private Button replayButtonUI;

        [Header("Panel")]
        [SerializeField] private GameObject pausePanelUI;

        [Header("Reference")]
        [SerializeField] private GameManager gameManager;

        #endregion

        #region Methods

        // !- Init
        protected override void InitOnStart()
        {
            base.InitOnStart();

            pauseButtonUI.onClick.AddListener(PauseGame);
            resumeButtonUI.onClick.AddListener(ResumeGame);
            replayButtonUI.onClick.AddListener(ReplayGame);

            pausePanelUI.SetActive(false);
        }

        // !- Core
        private void PauseGame()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            gameManager.SetGameRunning(false);
            pausePanelUI.SetActive(true);

            Time.timeScale = 0f;
        }

        private void ResumeGame()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            gameManager.SetGameRunning(true);
            pausePanelUI.SetActive(false);
            
            Time.timeScale = 1f;
        }

        private void ReplayGame()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.CurrentLevel);
        }

        #endregion
    }
}