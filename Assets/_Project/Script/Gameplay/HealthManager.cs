using UnityEngine;
using UnityEngine.UI;
using Defend.Enum;
using Defend.Item;
using Defend.Events;
using Defend.Database;
using Defend.Rhythm;
using System.Drawing;
using System;

namespace Defend.Gameplay
{
    public class HealthManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private float playerHealth;
        [SerializeField] private float lerpSpeed;
        
        private float _currentHealth;
        private bool _canModifyHealth;

        // Const variable
        private const float MAX_FILL_BAR = 1f;
        private const float MIN_FILL_BAR = 0f;

        [Header("UI")]
        [SerializeField] private Slider healthSlider;
        
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
            _canModifyHealth = true;
            _currentHealth = playerHealth;
            healthSlider.value = MAX_FILL_BAR;
        }
        
        private void Update()
        {
            if (!GameManager.IsGameRunning) return;
            
            var currentValue = _currentHealth / playerHealth;
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentValue, lerpSpeed * Time.deltaTime);

            // Player die
            if (healthSlider.value <= 0.01f )
            {
                healthSlider.value = MIN_FILL_BAR;
                GameEvents.GameEndEvent();
                Debug.Log("game end");
            }
        }

        #endregion
        
        #region Method
        
        private void ModifyHealth(Ball ball, DeflectStatus status)
        {
            if (!_canModifyHealth) return;

            var data = BallDatabase.Instance.GetDataByType(ball.Type);
            var point = data.GetHealthPoint(status);

            _currentHealth += point;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, playerHealth);
            if (_currentHealth <= 0f)
            {
                _currentHealth = 0f;
                _canModifyHealth = false;
            }
        }

        #endregion
    }
}
