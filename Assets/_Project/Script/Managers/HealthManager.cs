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
        [SerializeField] private float fillDuration;
        [SerializeField] private Slider healthSlider;
        
        private float _currentHealth;

        // Const variable
        private const float MAX_FILL_BAR = 1f;
        private const float MIN_FILL_BAR = 0f;

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
            healthSlider.value = MAX_FILL_BAR;
        }
        
        private void Update()
        {
            if (!GameManager.IsGameRunning) return;
            
            var currentValue = _currentHealth / playerHealth;
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentValue, fillDuration * Time.deltaTime);
            
            // Player die
            if (healthSlider.value <= MIN_FILL_BAR)
            {
                healthSlider.value = MIN_FILL_BAR;
                GameEvents.GameEndEvent();
            }
        }

        #endregion

        #region Method

        private void ModifyHealth(Ball ball, DeflectStatus status)
        {
            if (_currentHealth >= playerHealth) return;

            var data = BallDatabase.Instance.GetDataByType(ball.Type);
            var point = data.GetHealthPoint(status);
            _currentHealth += point;
        }

        #endregion
    }
}
