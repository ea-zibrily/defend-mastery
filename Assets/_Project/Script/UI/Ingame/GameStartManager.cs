using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Defend.Events;

namespace Defend.UI
{
    public class GameStartManager : MonoBehaviour
    {
        [Header("Start Game")]
        [SerializeField] private int countdownValue;
        [SerializeField] private float fadeDuration;
        [SerializeField] private TextMeshProUGUI countdownUI;

        private int _currentCount;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _currentCount = countdownValue;
            StartCoroutine(CountdownRoutine());

        }

        private IEnumerator CountdownRoutine()
        {
            countdownUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            while (_currentCount > 0)
            {
                countdownUI.text = _currentCount.ToString();
                yield return new WaitForSeconds(1f);
                _currentCount--;
            }

            countdownUI.text = "gaskan";
            yield return new WaitForSeconds(0.5f);
            FadeIn();
        }

        private void FadeIn()
        {
            _canvasGroup.gameObject.SetActive(true);
            
            _canvasGroup.DOFade(1f, 0f);
            _canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                // Reset
                _currentCount = countdownValue;
                _canvasGroup.interactable = true;
                _canvasGroup.gameObject.SetActive(false);
            });
            
            // Start game
            GameEvents.GameStartEvent();
        }

    }
}
