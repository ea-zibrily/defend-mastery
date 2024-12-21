using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Defend.Rhythm
{
    public class SongTrackerUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private int countdownValue;
        [SerializeField] private float fadeDuration;
        [SerializeField] private TextMeshProUGUI countdownText;
        [SerializeField] private TextMeshProUGUI songTitleText;
        [SerializeField] private CanvasGroup canvasGroup;

        private int _currentCount;

        // Reference
        private SongTracker _songTracker;

        private void Awake()
        {
            _songTracker = GetComponent<SongTracker>();
        }

        private void Start()
        {
            _currentCount = countdownValue;
            songTitleText.text = _songTracker.SongTitle;
            StartCoroutine(CountdownRoutine());
        }

        private IEnumerator CountdownRoutine()
        {
            countdownText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            while (_currentCount > 0)
            {
                countdownText.text = _currentCount.ToString();
                yield return new WaitForSeconds(1f);
                _currentCount--;
            }

            countdownText.text = "gaskan";
            yield return new WaitForSeconds(0.5f);
            FadeIn();
        }

        private void FadeIn()
        {
            canvasGroup.gameObject.SetActive(true);
            
            canvasGroup.DOFade(1f, 0f);
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                // Reset
                _currentCount = countdownValue;
                canvasGroup.interactable = true;
                canvasGroup.gameObject.SetActive(false);
            });
            
            // Start track
            _songTracker.StartTrack();
        }
    }
}
