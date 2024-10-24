using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Defend.Enum;
using Defend.Item;
using Defend.Events;
using Defend.Database;

namespace Defend.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private float currentScore;
        [SerializeField] private Sprite[] statusSprites;

        public float CurrentScore => currentScore;

        [Header("Indicator")]
        [SerializeField] private Image scoreIndicator;
        [SerializeField] private TextMeshProUGUI scoreTextUI;

        [Header("Tweening")]
        [SerializeField] private Ease easeType;
        [SerializeField] private float easeDuration;
        [SerializeField] private float tweenDuration;
        [SerializeField] private Vector3 scaleNormal;
        [SerializeField] private Vector3 scaleTarget;

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
            scoreIndicator.transform.localScale = Vector3.zero;
            scoreIndicator.gameObject.SetActive(false);
        }

        #endregion

        #region Methods

        private void ModifyScore(Ball ball, DeflectStatus status)
        {
            // Add score
            AddScore(ball, status);

            // Add animation
            if (ball.Type == BallType.Bom) return;
            AnimateIndicator(status);
        }

        private void AddScore(Ball ball, DeflectStatus status)
        {
            var data = BallDatabase.Instance.GetDataByType(ball.Type);

            currentScore += data.ScorePoints[(int)status];
            scoreTextUI.text = currentScore.ToString();
        }
        
        private void AnimateIndicator(DeflectStatus status)
        {
            if (!scoreIndicator.gameObject.activeSelf)
                scoreIndicator.gameObject.SetActive(true);

            var scoreRect = scoreIndicator.GetComponent<RectTransform>();

            scoreRect.DOScale(Vector3.zero, 0f);
            scoreIndicator.sprite = statusSprites[(int)status];
            scoreIndicator.SetNativeSize();

            scoreRect.DOScale(scaleTarget, easeDuration).SetEase(easeType);
            scoreRect.DOScale(scaleNormal, easeDuration).SetEase(easeType).
                SetDelay(tweenDuration).OnComplete(() => 
                {
                     scoreIndicator.sprite = null;
                });
        }

        #endregion
    }
}