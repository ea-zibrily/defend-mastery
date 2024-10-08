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
        [SerializeField] private List<SongTimes> musicTrackTimes;

        private int _trackIndex;
        private float _currentSec;
        private float _currentBeat;
        private float _tempBeat;
        private float _startTime;
        private float _firstStartTiming;

        private const int NORMAL_BALL_PERCENT = 85;
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
            // Init track
            _trackIndex = 0;
            _firstStartTiming = 0f;
            _currentBeat = SECOND_PER_MIN / musicData.SongBpm;

            // Init audio
            musicTrackTimes = musicData.SongTimes;
            _audioSource.clip = musicData.SongClip;
            if (!_audioSource.loop) 
                _audioSource.loop =  true;
               
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
                if (_trackIndex >= musicTrackTimes.Count) return;

                var track = musicTrackTimes[_trackIndex];
                if (_trackIndex == 0)
                {
                    if (_firstStartTiming != 0)
                        _firstStartTiming = _currentSec;
                    _tempBeat = CalculateTiming(_currentSec);
                }
                
                // Spawn ball
                var type = GetRandomBall(track.IsSuper);
                ballSpawner.SpawnBall(type, track.Duration, _trackIndex == 0 ? 1 : 0);

                // Set beat
                _currentSec = _currentBeat;
                _currentBeat = track.Timing + (_tempBeat != 0 ? -0.15f : 0);
                _trackIndex++;
            }
        }

        // !- Helpers
        private float CalculateTiming(float currentSec)
        {
            var difference = Mathf.Abs(_firstStartTiming - currentSec);
            return difference * (_firstStartTiming > currentSec ? 1 : -1);
        }

        private void PlaySong()
        {
            _startTime = Time.time;
            _audioSource.Play();
        }

        private void ReplaySong()
        {
            _currentSec = 0f;
            _startTime = Time.time;                
        }

        private void StopSong()
        {
            _startTime = 0f;
            _audioSource.Stop();
        }

        private BallType GetRandomBall(bool isSuper)
        {
            if (isSuper) return BallType.Super;

            var percent = (float)NORMAL_BALL_PERCENT / 100;
            return Random.value < percent ? BallType.Normal : BallType.Bom;
        }

        #endregion
    }
}
