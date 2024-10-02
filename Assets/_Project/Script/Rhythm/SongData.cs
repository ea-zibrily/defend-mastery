using System.Collections.Generic;
using UnityEngine;

namespace Defend.Rhythm
{
    [CreateAssetMenu(fileName = "NewSongData", menuName = "Data/Song Data", order = 0)]
    public class SongData : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private string songTitle;
        [SerializeField] private string songArtist;
        [SerializeField] private float songDuration;

        [Header("Stats")]
        [SerializeField] private int songBpm;
        [SerializeField] private AudioClip songClip;
        [SerializeField] private List<SongTimes> songTimes;

        // TODO: Redundant method, drop later
        [Space]
        [SerializeField] private List<float> normalTime;
        [SerializeField] private List<float> superTime;
        [SerializeField] private List<float> bombTime;

        // Getter
        public float SongDuration => songDuration;
        public int SongBpm => songBpm;
        public AudioClip SongClip => songClip;
        public List<SongTimes> SongTimes=> songTimes;


        // TODO: Redundant method, drop later
        public List<float> NormalTime => normalTime;
        public List<float> SuperTime => superTime;
        public List<float> BombTime => bombTime;
    }
}