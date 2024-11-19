using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Defend.Enum;
using Defend.Audio;
using Defend.Database;
using Defend.Managers;

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
            GameDatabase.Instance.SetReplay(true);
            SceneTransitionManager.Instance.LoadSelectedScene(SceneState.CurrentLevel);
        }

        #endregion
    }
}
