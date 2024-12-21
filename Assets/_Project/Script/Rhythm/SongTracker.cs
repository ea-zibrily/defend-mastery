using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Defend.Rhythm
{
    public class SongTracker : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Data")]
        [SerializeField] private AudioClip songClip;
        [SerializeField] private string songTitle;
        [SerializeField] private int songBpm;
        [ReadOnly] [SerializeField] private float secPerBeat;
        [ReadOnly] [SerializeField] private float songDuration;
        [ReadOnly] [SerializeField] [Searchable] private List<SongTimes> songTimes;

        private float _currentTime;
        private float _currentSuperTime;
        private float _currentDuration;
        private float _startTime;
        private bool _canTrackBeat;
        public string SongTitle => songTitle;

        // Reference
        private AudioSource _audioSource;

        #endregion

        #region MonoBehaviour Callbacks
        
        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        private void Start()
        {
            _currentTime = 0;
            _canTrackBeat = false;
            _audioSource.clip = songClip;

            secPerBeat = 60f / songBpm;
            songDuration = _audioSource.clip.length;
        }
        
        private void Update()
        {
            if (!_canTrackBeat) return;
            HandleTracker();
        }

        // !- Methods
        public void StartTrack()
        {
            _startTime = Time.time;
            _canTrackBeat = true;
            _audioSource.Play();
        }

        private void HandleTracker()
        {
            if (_currentTime >= songDuration) return;

            _currentTime = Time.time - _startTime;

            if (Input.GetKeyDown(KeyCode.A))
            {
                SongTimes time = new(_currentTime);
                songTimes.Add(time);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _currentSuperTime = _currentTime;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                _currentDuration += Time.deltaTime;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                // Add times
                SongTimes superTime = new(_currentSuperTime, _currentDuration, isSuper: true);
                songTimes.Add(superTime);

                // Reset
                _currentDuration = 0f;
                _currentSuperTime = 0f;
            }
        }
        
        #endregion
    }
}