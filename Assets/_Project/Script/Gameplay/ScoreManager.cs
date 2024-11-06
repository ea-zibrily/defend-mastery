using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Defend.Enum;
using Defend.Item;
using Defend.Events;
using Defend.Database;

namespace Defend.Gameplay
{
    public class ScoreManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private float currentScore;
        [SerializeField] private Sprite[] pointSprites;
        [SerializeField] private Sprite[] statusSprites;

        public float CurrentScore => currentScore;

        [Header("Indicator")]
        [SerializeField] private Image scorePoint;
        [SerializeField] private Image scoreStatus;
        [SerializeField] private TextMeshProUGUI scoreTextUI;
        [SerializeField] private TextMeshProUGUI scorePointTextUI;

        [Header("Tweening")]
        [SerializeField] private Ease easeType;
        [SerializeField] private float easeDuration;
        [SerializeField] private float tweenDuration;
        [SerializeField] private Vector3 scaleNormal;
        [SerializeField] private Vector3 scaleTarget;

        [Header("Reference")]
        [SerializeField] private ComboManager comboManager;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            GameEvents.OnDeflectBall += ModifyScore;
        }

        private void OnDisable()
        {
            GameEvents.OnDeflectBall -= ModifyScore;
        }

        private void Start()
        {
            currentScore = 0f;
            scoreTextUI.text = currentScore.ToString();

            // Init score ui
            scorePoint.transform.localScale = Vector3.zero;
            scorePoint.gameObject.SetActive(false);

            scoreStatus.transform.localScale = Vector3.zero;
            scoreStatus.gameObject.SetActive(false);
        }

        #endregion

        #region Methods
        
        private void ModifyScore(Ball ball, DeflectStatus status)
        {
            // Add score
            AddScore(ball, status);

            // Animate indicator
            if (ball.Type != BallType.Bom)
            {
                AnimateIndicator(status, scoreStatus, statusSprites);
                if (status != DeflectStatus.Miss)
                {
                    AnimateIndicator(status, scorePoint, pointSprites);
                }
            }
        }
        
        private void AddScore(Ball ball, DeflectStatus status)
        {
            var data = BallDatabase.Instance.GetDataByType(ball.Type);
            var point = data.GetScorePoint(status) * comboManager.ComboMultiplier;
            
            currentScore += point;
            scoreTextUI.text = currentScore.ToString();
            scorePointTextUI.text = point.ToString();
        }
        
        private void AnimateIndicator(DeflectStatus status, Image indicator, Sprite[] sprites)
        {
            if (!indicator.gameObject.activeSelf)
                indicator.gameObject.SetActive(true);

            var scoreRect = indicator.GetComponent<RectTransform>();

            scoreRect.DOScale(Vector3.zero, 0f);
            indicator.sprite = sprites[(int)status];
            indicator.SetNativeSize();
            
            scoreRect.DOScale(scaleTarget, easeDuration).SetEase(easeType);
            scoreRect.DOScale(scaleNormal, easeDuration).SetEase(easeType).SetDelay(tweenDuration);
        }

        #endregion
    }
}