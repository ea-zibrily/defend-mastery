using System;
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
        [SerializeField] private List<SongPhase> songPhases;

        // Getter
        public float SongDuration => songDuration;
        public int SongBpm => songBpm;
        public AudioClip SongClip => songClip;
        public List<SongPhase> SongPhases=> songPhases;
    }
}