using Defend.Audio;
using Defend.Database;
using Defend.Enum;
using Defend.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Defend.UI
{
    public class GameEndManager : GameUI
    {
        #region Fields & Properties

        // UI
        [SerializeField] private Button replayButtonUI;
        [SerializeField] private TextMeshProUGUI scoreTextUI;

        #endregion

        #region Methods

        protected override void InitOnStart()
        {
            base.InitOnStart();

            var score = GameDatabase.Instance.GetHighScore();
            scoreTextUI.text = "Score: " + score;
            replayButtonUI.onClick.AddListener(ReplayGame);
        }

        private void ReplayGame()
        {
            AudioManager.Instance.PlayAudio(Musics.ButtonSfx);
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.CurrentLevel);
        }

        #endregion

        private bool isPressedReplay;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isPressedReplay)
            {
                isPressedReplay = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }
}
