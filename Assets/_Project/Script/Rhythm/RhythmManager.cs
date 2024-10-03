using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Defend.Enum;
using Defend.Item;
using Defend.Events;
using Defend.Rhythm;

namespace Defend.Managers
{
    public class RhythmManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private SongData musicData;
        [SerializeField] [Searchable] private List<SongTimes> musicTrackTimes;

        private int _trackIndex;
        private float _currentSec;
        private float _currentBeat;

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
            _trackIndex = 0;
            _currentSec = 0f;
            _currentBeat = SECOND_PER_MIN / musicData.SongBpm;
            musicTrackTimes = musicData.SongTimes;

            // Play audio
            _audioSource.clip = musicData.SongClip;
            _audioSource.Play();
        }

        private void Update()
        {
            if (!GameManager.IsGameRunning) return;

            if (_currentSec >= musicData.SongDuration)
                GameEvents.GameEndEvent();

            _currentSec += Time.deltaTime;
            if (_currentSec >= _currentBeat)
            {
                if (_trackIndex >= musicTrackTimes.Count) return;
                var track = musicTrackTimes[_trackIndex];

                // Spawn ball
                var type = GetRandomBall(track.IsSuper);
                ballSpawner.SpawnBall(type, track.Duration, _trackIndex == 0 ? 1 : 0);

                // Set beat
                _currentSec = _currentBeat;
                _currentBeat = track.Timing;
                _trackIndex++;
            }
        }

        #endregion

        #region Methods

        private BallType GetRandomBall(bool isSuper)
        {
            if (isSuper) return BallType.Super;
            return Random.value < 0.85 ? BallType.Normal : BallType.Bom;
        }

        #endregion
    }
}
