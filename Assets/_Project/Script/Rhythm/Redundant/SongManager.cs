using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Defend.Rhythm
{
    public class SongManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private SongData musicData;
        [SerializeField] [Searchable] private List<float> musicTrackTimez;
        [SerializeField] [Searchable] private List<SongTimes> musicTrackTimes;

        [Header("Reference")]
        [SerializeField] private SongBall songBall;
        [SerializeField] private Transform spawnBall;
        [SerializeField] private Transform spawnParent;
        private AudioSource _audioSource;

        private float _currentSec;
        private float _currentBeat;
        private int _trackIndex;

        private float _startTime;

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
            _currentBeat = 60f / musicData.SongBpm;
            _startTime = Time.time;

            // Merge track
            List<float> basicTrack = MergeSortedLists(musicData.NormalTime, musicData.BombTime);
            musicTrackTimez = MergeSortedLists(basicTrack, musicData.SuperTime);

            musicTrackTimes = musicData.SongTimes;

            // Play audio
            _audioSource.clip = musicData.SongClip;
            _audioSource.Play();
        }

        private void Update()
        {
            if (_currentSec >= musicData.SongDuration) return;
            if (_trackIndex >= musicTrackTimes.Count) return;
            
            // _currentSec += Time.deltaTime;
            _currentSec = Time.time - _startTime;

            if (_currentSec >= _currentBeat)
            {
                var track = musicTrackTimes[_trackIndex];

                // Spawn ball
                var ball = Instantiate(songBall, spawnBall.position, Quaternion.identity);
                ball.transform.parent = spawnParent;
                if (_trackIndex == 0)
                    ball.BallSpeed = 11f;
                if (track.IsSuper)
                {
                    Debug.Log("super!");
                    ball.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                }

                // Set beat
                _currentSec = _currentBeat;
                _currentBeat = track.Timing;
                _trackIndex++;
                // _currentBeat = musicTrackTimez[_trackIndex];
            }
        }
        
        #endregion

        #region Methods

        private List<float> MergeSortedLists(List<float> listA, List<float> listB)
        {
            List<float> outputList = new();

            var i = 0;
            var j = 0;

            while (i < listA.Count && j < listB.Count)
            {
                if (listA[i] < listB[j])
                {
                    outputList.Add(listA[i]);
                    i++;
                }
                else
                {
                    outputList.Add(listB[j]);
                    j++;
                }
            }

            while (i < listA.Count)
            {
                outputList.Add(listA[i]);
                i++;
            }

            while (j < listB.Count)
            {
                outputList.Add(listB[j]);
                j++;
            }

            return outputList;
        }

        #endregion
    }
}