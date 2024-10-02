using System;
using UnityEngine;

namespace Defend.Rhythm
{
    [Serializable]
    public class SongTimes
    {
        public float Timing;
        public float Duration;
        public bool IsSuper;

        public SongTimes(float time, float duration = 0, bool isSuper = false)
        {
            Timing = time;
            Duration = duration;
            IsSuper = isSuper;
        }
    }
}