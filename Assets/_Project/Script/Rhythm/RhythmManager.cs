using System.Collections.Generic;
using UnityEngine;
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
        [SerializeField] private float spawnTimingDec;
        [SerializeField] private List<SongTimes> musicTracks;

        private float _secPerBeat;
        private int _trackIndex;
        private float _currentSec;
        private float _currentBeat;
        private float _startTime;

        private const float NORMAL_BALL_PERCENT = 85f;
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

        private void OnEnable()
        {
            GameEvents.OnGameStart += PlaySong;
            GameEvents.OnGameEnd += StopSong;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= PlaySong;
            GameEvents.OnGameEnd -= StopSong;
        }

        private void Start()
        {
            // Init
            _trackIndex = 0;
            _currentSec = 0f;
            _secPerBeat = SECOND_PER_MIN / musicData.SongBpm * 2f;
            
            musicTracks = musicData.SongTimes;
            _currentBeat = musicTracks[_trackIndex].Timing - _secPerBeat;
            _currentBeat -= GameDatabase.Instance.IsFirstPlay ? 0.18f: 0;
            _audioSource.clip = musicData.SongClip;
        }

        private void Update()
        {
            if (!GameManager.IsGameRunning) return;
            
            if (_currentSec >= musicData.SongDuration)
                ReplaySong();
            
            HandleTrack();
        }

        #endregion

        #region Methods

        // !- Core
        private void HandleTrack()
        {
            _currentSec = Time.time - _startTime;
            if (_currentSec >= _currentBeat)
            {
                _trackIndex++;                
                if (_trackIndex >= musicTracks.Count) return;
                var track = musicTracks[_trackIndex];
                
                // Spawn ball
                var type = GetRandomBall(track.IsSuper);
                ballSpawner.SpawnBall(type, track.Duration, 0f);

                // Set beat
                _currentSec = _currentBeat;
                _currentBeat = track.Timing - _secPerBeat;
                _currentBeat -= GameDatabase.Instance.IsFirstPlay ? 0.18f: 0;
            }
        }

        // !- Helpers
        private void PlaySong()
        {
            _trackIndex = 0;
            _startTime = Time.time;
            _audioSource.Play();
        }

        private void ReplaySong()
        {
            _trackIndex = 0;
            _currentSec = 0f;
            _startTime = Time.time;

            _currentBeat = musicTracks[_trackIndex].Timing - _secPerBeat;
        }

        private void StopSong()
        {
            _startTime = 0f;
            _audioSource.Stop();
            GameDatabase.Instance.SetFirstPlay(true);
        }

        private BallType GetRandomBall(bool isSuper)
        {
            if (isSuper) return BallType.Super;

            var percent = NORMAL_BALL_PERCENT / 100;
            return Random.value < percent ? BallType.Normal : BallType.Bom;
        }

        #endregion
    }
}
