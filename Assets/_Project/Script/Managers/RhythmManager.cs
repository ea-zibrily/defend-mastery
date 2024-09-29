using UnityEngine;
using Sirenix.OdinInspector;
using Defend.Item;
using Defend.Events;

namespace Defend.Managers
{
    public class RhythmManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private float musicBpm;
        [SerializeField] [ReadOnly] private float secPerBeat;

        private float _currentSec;
        private float _tempBeat;
        private const float SECOND_PER_MIN = 60f;

        [Header("Reference")]
        [SerializeField] private BallSpawner ballSpawner;
        private AudioSource _audioSource;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        private void Start()
        {
            // Init
            _currentSec = 0f;
            secPerBeat = SECOND_PER_MIN /  musicBpm;
            _tempBeat = secPerBeat * 3f;

            // Play audio
            _audioSource.Play();
        }

        private void Update()
        {
            StartBeat();
            HandleRhythm();
        }

        #endregion

        #region Methods

        private void StartBeat()
        {
            if (GameManager.IsGameRunning) return;
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("game start!");
                GameEvents.GameStartEvent();
            }
        }

        private void HandleRhythm()
        {
            _currentSec += Time.deltaTime;
            if (_currentSec >= _tempBeat)
            {
                // Reset sec
                _currentSec = 0f;

                // Spawn ball
                var isSuperBall = ballSpawner.IsSuperBall();
                if (!GameManager.IsGameRunning || isSuperBall) return;
                ballSpawner.SpawnBall();
            }
        }

        #endregion
    }
}
