using UnityEngine;
using UnityEngine.UI;
using Defend.Events;
using Defend.Managers;

namespace Defend.Gameplay
{
    public class HealthManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private float playerHealth;
        [SerializeField] private float healthDuration;
        
        private float _currentHealth;

        [Header("UI")]
        [SerializeField] private Image fillImageUI;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _currentHealth = playerHealth;
            fillImageUI.fillAmount = 1f;
        }

        private void Update()
        {
            if (!GameManager.IsGameRunning) return;

            var currentFill = _currentHealth / playerHealth;
            _currentHealth = Mathf.Max(_currentHealth - 1.5f * Time.deltaTime, 0f);
            fillImageUI.fillAmount = Mathf.Lerp(fillImageUI.fillAmount, currentFill, healthDuration * Time.deltaTime);
            
            // Player die
            if (fillImageUI.fillAmount <= 0.01f)
            {
                GameEvents.GameEndEvent();
            }
        }

        #endregion

        #region Method

        public void ModifyHealth(float value)
        {
            if (_currentHealth >= playerHealth) return;
            _currentHealth += value;
        }

        #endregion
    }
}
