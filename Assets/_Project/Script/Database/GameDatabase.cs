using Defend.Singleton;

namespace Defend.Database
{
    public class GameDatabase : MonoDDOL<GameDatabase>
    {
        // Fields
        private float _highScore;
        private bool _isReplay;

        // Methods
        public float GetHighScore() => _highScore;
        public void SetHighScore(float score) 
        {
            if (score < _highScore) return;
            _highScore = score;
        }

        public bool IsFirstPlay() => _isReplay;
        public void SetReplay(bool condition)
        {
            if (_isReplay == condition) return;
            _isReplay = condition;
        }
    }
}