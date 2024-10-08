using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Defend.Events;

namespace Defend.UI
{
    public class GameStartManager : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private int countdownValue;
        [SerializeField] private float fadeDuration;

        private bool _isCountdownStart;
        private int _currentCount;

        [Header("UI")]
        [SerializeField] private GameObject titleUI;
        [SerializeField] private GameObject tapToPlayUI;
        [SerializeField] private TextMeshProUGUI countdownUI;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _isCountdownStart = false;
            _currentCount = countdownValue;

            DOTween.Init(true, false, LogBehaviour.Verbose).SetCapacity(200, 10);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_isCountdownStart)
            {
                titleUI.SetActive(false);
                tapToPlayUI.SetActive(false);
                StartCoroutine(CountdownRoutine());
            }
        }

        private IEnumerator CountdownRoutine()
        {
            _isCountdownStart = true;
            countdownUI.gameObject.SetActive(true);

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
