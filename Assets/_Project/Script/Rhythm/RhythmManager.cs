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

        private int _trackIndex;
        private float _currentSec;
        private float _currentBeat;
        private float _tempBeat;
        private float _startTime;
        private float _firstStartTiming;

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
            // Init track
            _trackIndex = 0;
            _firstStartTiming = SongVision.Instance.GetStartTiming();
            _currentBeat = SECOND_PER_MIN / musicData.SongBpm;
            _currentBeat -= spawnTimingDec;

            // Init audio
            musicTracks = musicData.SongTimes;
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
                if (_trackIndex >= musicTracks.Count) return;

                var track = musicTracks[_trackIndex];
                
                // Spawn ball
                var type = GetRandomBall(track.IsSuper);
                ballSpawner.SpawnBall(type, track.Duration, 0f);

                // Set beat
                _currentSec = _currentBeat;
                _currentBeat = track.Timing;
                _trackIndex++;
            }
        }

        // private void HandleTrack()
        // {
        //     _currentSec = Time.time - _startTime;
        //     if (_currentSec >= _currentBeat)
        //     {
        //         if (_trackIndex >= musicTrackTimes.Count) return;

        //         var track = musicTrackTimes[_trackIndex];
        //         if (_trackIndex == 0)
        //         {
        //             if (_firstStartTiming != 0)
        //             {
        //                 _firstStartTiming = _currentSec;   
        //                 SongVision.Instance.SetStartTiming(_currentSec);
        //             }
        //             _tempBeat = CalculateTiming(_currentSec);
        //         }
                
        //         // Spawn ball
        //         var type = GetRandomBall(track.IsSuper);
        //         ballSpawner.SpawnBall(type, track.Duration, _trackIndex == 0 ? 1 : 0);

        //         // Set beat
        //         _currentSec = _currentBeat;
        //         _currentBeat = track.Timing + (_tempBeat != 0 ? -0.2f : 0.2f);
        //         _trackIndex++;
        //     }
        // }

        // !- Helpers
        private float CalculateTiming(float currentSec)
        {
            var difference = Mathf.Abs(_firstStartTiming - currentSec);
            return difference * (_firstStartTiming > currentSec ? 1 : -1);
        }

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

            _firstStartTiming = SongVision.Instance.GetStartTiming();
            _currentBeat = SECOND_PER_MIN / musicData.SongBpm;
            _currentBeat -= spawnTimingDec;
        }

        private void StopSong()
        {
            _startTime = 0f;
            _audioSource.Stop();
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
