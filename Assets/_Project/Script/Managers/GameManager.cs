using UnityEngine;
using Defend.Events;
using Defend.Database;

namespace Defend.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("UI")]
        [SerializeField] private GameObject gameResultPanel;

        [Header("Reference")]
        [SerializeField] private ScoreManager scoreManager;
        
        // Misc
        public static bool IsGameRunning { get; private set;}
        
        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            GameEvents.OnGameStart += GameStart;
            GameEvents.OnGameEnd += GameEnd;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= GameStart;
            GameEvents.OnGameEnd -= GameEnd;
        }

        private void Start()
        {
            gameResultPanel.SetActive(false);
        }

        #endregion

        #region Methods
        
        // !- Core
        private void GameStart()
        {
            IsGameRunning = true;
        }
        
        private void GameEnd()
        {
            IsGameRunning = false;
            GameDatabase.Instance.SetHighScore(scoreManager.CurrentScore);
            gameResultPanel.SetActive(true);
        }
        
        // !- Helpers
        public void SetGameRunning(bool isRunning)
        {
            IsGameRunning = isRunning;
        }
        
        #endregion
    }
}
