using Defend.Singleton;

namespace Defend.Rhythm
{
    public class SongVision : MonoDDOL<SongVision>
    {
        private float _firstStartTiming;
        public void SetStartTiming(float timing) => _firstStartTiming = timing;        
        public float GetStartTiming() => _firstStartTiming;
        
    }
}