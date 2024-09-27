using UnityEngine;
using Defend.Events;

namespace Defend.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("UI")]
        [SerializeField] private GameObject gameResultPanel;

        // Misc
        public static bool IsGameRunning { get; private set;}
        
        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            GameEvents.OnGameStart += GameStart;
            GameEvents.OnGameEnd -= GameEnd;
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
            // Stats
            IsGameRunning = false;
            gameResultPanel.SetActive(true);
        }

        #endregion
    }
}
