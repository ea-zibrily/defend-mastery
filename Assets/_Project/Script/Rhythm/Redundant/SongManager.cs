using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Collections;

namespace Defend.Rhythm
{
    public class SongManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private SongData musicData;
        [SerializeField] private int tempPhase;
        [ReadOnly] [SerializeField] [Searchable] private List<SongTimes> musicTracks;

        private float _currentSec;
        private float _currentBeat;
        private float _secPerBeat;
        private int _trackIndex;

        private float _startTime;
        private bool _isStartBeating;

        [Header("Reference")]
        [SerializeField] private SongBall songBall;
        [SerializeField] private Transform spawnBall;
        [SerializeField] private Transform spawnParent;
        private AudioSource _audioSource;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        private void OnEnable()
        {
            DOTween.Init(true, false, LogBehaviour.Verbose).SetCapacity(900, 360);
        }

        private void Start()
        {
            // Init
            _trackIndex = 0;
            _currentSec = 0f;
            _secPerBeat = 60f / musicData.SongBpm * 2f;

            musicTracks = musicData.SongPhases[tempPhase].Times;
            _currentBeat = musicTracks[_trackIndex].Timing - _secPerBeat;

            // Play audio
            _audioSource.clip = musicData.SongClip;
            StartCoroutine(CountdownRoutine());
        }
        
        private void Update()
        {
            if (!_isStartBeating) return;

            if (_currentSec >= musicData.SongDuration) return;
            if (_trackIndex >= musicTracks.Count) return;
            
            _currentSec = Time.time - _startTime;
            if (_currentSec >= _currentBeat)
            {
                var track = musicTracks[_trackIndex];

                // Spawn ball
                var ball = Instantiate(songBall, spawnBall.position, Quaternion.identity);
                ball.transform.parent = spawnParent;
                ball.Move();

                if (track.IsSuper)
                {
                    Debug.Log("super!");
                    ball.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                }

                // Set beat
                _currentSec = _currentBeat;
                _currentBeat = track.Timing - _secPerBeat;
                _trackIndex++;
            }
        }

        private IEnumerator CountdownRoutine()
        {
            _isStartBeating = false;
            yield return new WaitForSeconds(3f);

            _isStartBeating = true;
            _startTime = Time.time;
            _audioSource.Play();
            Debug.Log("go!");
        }
        
        #endregion
    }
}