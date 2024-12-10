using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Defend.Enum;
using Defend.Item;
using Defend.Events;
using Defend.Managers;

namespace Defend.Gameplay
{
    public class ComboManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private int[] comboHits;
        [SerializeField] private float lerpSpeed;
        
        private int _currentHits;
        private int _currentCombo;
        public int ComboMultiplier => _currentCombo;
        private readonly int MinCombo = 1;
        private readonly int MaxCombo = 4;

        [Header("UI")]
        [SerializeField] private Image comboImageUI;
        [SerializeField] private TextMeshProUGUI comboTextUI;
        
        #endregion

        #region MonoBehaviour Callback

        private void OnEnable()
        {
            GameEvents.OnDeflectBall += ModifyCombo;
        }

        private void OnDisable()
        {
            GameEvents.OnDeflectBall -= ModifyCombo;
        }

        private void Start()
        {
            // Validate
            if (comboHits.Length != MaxCombo - 1)
            {
                Debug.LogError("combo hits kurang!");
                return;
            }
            
            _currentHits = 0;
            _currentCombo = MinCombo;
            comboImageUI.fillAmount = 0;
            comboTextUI.text = _currentCombo + "X";
        }

        private void Update()
        {
            if (!GameManager.IsGameRunning) return;

            HandleComboSlider();
            comboTextUI.text = _currentCombo + "X";
        }
        
        #endregion

        #region Methods

        // !- Core
        private void HandleComboSlider()
        {
            var comboIndex = GetComboIndex();
            var currentValue = (float)_currentHits / comboHits[comboIndex];
            
            comboImageUI.fillAmount = Mathf.Lerp(comboImageUI.fillAmount, currentValue, lerpSpeed * Time.deltaTime);
        }
        
        private void ModifyCombo(Ball ball, DeflectStatus status)
        {
            if (ball.Type == BallType.Bom) return;

            if (status == DeflectStatus.Perfect)
            {
                IncreaseHit();
            }
            else if (status == DeflectStatus.Miss)
            {
                DecreaseHit();
            }
        }

        private void IncreaseHit()
        {
            if (_currentHits >= comboHits[MaxCombo - 2]) return;

            _currentHits++;
            if (ShouldIncreaseCombo())
            {
                _currentCombo++;
                _currentCombo = Mathf.Clamp(_currentCombo, 1, MaxCombo);
                
                if (_currentCombo < MaxCombo)
                {
                    _currentHits = 0;
                }
            }
        }

        private void DecreaseHit()
        {
            if (_currentCombo > MinCombo)
            {
                _currentCombo--;
                _currentHits = _currentCombo == MinCombo ?
                    0 : comboHits[_currentCombo - 2];
            }
            else
            {
                _currentHits = 0;
            }
        }
        
        // !- Helpers
        private bool ShouldIncreaseCombo()
        {
            var comboIndex = GetComboIndex();
            return _currentHits >= comboHits[comboIndex];
        }

        private int GetComboIndex()
        {
            return Mathf.Clamp(_currentCombo - 1, 0, MaxCombo - 2);
        }
        
        #endregion
    }
}