using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Defend.Item;
using Defend.Enum;
using Defend.Events;
using Defend.Database;

namespace Defend.Rhythm
{
    public class RhythmPhase : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Stats")]
        [SerializeField] private SongData musicData;
        [SerializeField] private float beatBalancer = 0.13f;
        [ReadOnly] [SerializeField] private List<SongPhase> musicTimelines;
        
        [SerializeField] private int _tempLoop;
        [SerializeField] private int _phaseIndex;
        private int _trackIndex;
        private bool _isTrackActive;
        private List<SongTimes> _currentTrack;
        
        private float _secPerBeat;
        private float _currentSec;
        private float _currentBeat;
        private float _startTime;
        
        // Cached variable
        private const int MAX_LOOP_TRACK = 3;
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
            // Neccesary
            _tempLoop = 0;
            _phaseIndex = 0;
            _trackIndex = 0;
            _currentSec = 0f;

            _isTrackActive = false;
            _secPerBeat = SECOND_PER_MIN / musicData.SongBpm * 2;
            
            // Rhythm track
            musicTimelines = musicData.SongPhases;
            _currentTrack = musicTimelines[_phaseIndex].Times;
            _currentBeat = _currentTrack[_trackIndex].Timing - _secPerBeat;
            _currentBeat -= GameDatabase.Instance.IsFirstPlay() ? beatBalancer : 0f;
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

            if (!_isTrackActive) return;
            if (_currentSec >= _currentBeat)
            {                
                // Handle loop & track
                _trackIndex++;
                if (_trackIndex >= _currentTrack.Count)
                {
                    HandleLoopTrack();
                    return;
                }
                
                var track = _currentTrack[_trackIndex];
                
                // Spawn ball
                var type = GetRandomBall(track.IsSuper);
                ballSpawner.SpawnBall(type, track.Duration);
                
                // Set beat
                _currentSec = _currentBeat;
                _currentBeat = track.Timing - _secPerBeat;
                _currentBeat -= GameDatabase.Instance.IsFirstPlay() ? beatBalancer : 0f;
            }
        }

        private void HandleLoopTrack()
        {
            _tempLoop++;
            _isTrackActive = false;
            if (_tempLoop >= MAX_LOOP_TRACK)
            {
                _tempLoop = 0;
                _phaseIndex = Mathf.Clamp(++_phaseIndex, 0, musicTimelines.Count - 1);
                _currentTrack = musicTimelines[_phaseIndex].Times;
            }
        }
        
        // !- Helpers
        private void PlaySong()
        {
            _trackIndex = 0;
            _isTrackActive = true;
            
            _startTime = Time.time;
            _audioSource.Play();
        }
        
        private void ReplaySong()
        {
            _trackIndex = 0;
            _currentSec = 0f;
            _isTrackActive = true;

            _startTime = Time.time;
            _currentBeat = _currentTrack[_trackIndex].Timing - _secPerBeat;
            Debug.Log("replay!");
        }

        private void StopSong()
        {
            _startTime = 0f;
            _audioSource.Stop();
        }

        public void PauseSong()
        {
            _audioSource.Pause();
        }

        public void UnpauseSong()
        {
            _audioSource.UnPause();
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
