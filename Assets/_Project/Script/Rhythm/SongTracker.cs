using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Defend.Rhythm
{
    public class SongTracker : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Data")]
        [SerializeField] private int songBpm;
        [SerializeField] private float secPerBeat;
        [SerializeField] private float songDuration;
        [SerializeField] [Searchable] private List<SongTimes> songTimes;

        private float _currentTime;
        private float _currentSuperTime;
        private float _currentDuration;
        private float _startTime;

        [Header("Reference")]
        [SerializeField] private AudioSource audioSource;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _currentTime = 0;
            _startTime = Time.time;

            secPerBeat = 60f / songBpm;
            songDuration = audioSource.clip.length;
        }

        private void Update()
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