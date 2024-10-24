using UnityEngine;
using UnityEngine.UI;
using Defend.Enum;
using Defend.Item;
using Defend.Events;
using Defend.Database;

namespace Defend.Managers
{
    public class HealthManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private float playerHealth;
        [SerializeField] private float healthMultiplier;
        [SerializeField] private float fillDuration;
        
        private float _currentHealth;

        [Header("UI")]
        [SerializeField] private Image fillImageUI;
        
        #endregion

        #region MonoBehaviour Callbacks
        
        private void OnEnable()
        {
            GameEvents.OnDeflectBall += ModifyHealth;
        }

        private void OnDisable()
        {
            GameEvents.OnDeflectBall -= ModifyHealth;
        }

        private void Start()
        {
            _currentHealth = playerHealth;
            fillImageUI.fillAmount = 1f;
        }

        private void Update()
        {
            if (!GameManager.IsGameRunning) return;

            var currentFill = _currentHealth / playerHealth;
            _currentHealth = Mathf.Max(_currentHealth - healthMultiplier * Time.deltaTime, 0f);
            fillImageUI.fillAmount = Mathf.Lerp(fillImageUI.fillAmount, currentFill, fillDuration * Time.deltaTime);
            
            // Player die
            if (fillImageUI.fillAmount <= 0.01f)
            {
                GameEvents.GameEndEvent();
            }
        }

        #endregion

        #region Method

        private void ModifyHealth(Ball ball, DeflectStatus status)
        {
            if (_currentHealth >= playerHealth) return;

            var data = BallDatabase.Instance.GetDataByType(ball.Type);
            float value;
            
            if (status == DeflectStatus.Perfect || status == DeflectStatus.Good)
                value = data.HealthPoints[0];
            else
                value = data.HealthPoints[1];
            
            _currentHealth += value;
        }

        #endregion
    }
}
