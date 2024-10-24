using Defend.Singleton;

namespace Defend.Database
{
    public class GameDatabase : MonoDDOL<GameDatabase>
    {
        // Fields
        private float _highScore;
        private bool _isFirstPlay;

        // Methods
        public float GetHighScore() => _highScore;
        public void SetHighScore(float score) => _highScore = score;

        public bool IsFirstPlay() => _isFirstPlay;
        public void SetFirstPlay(bool condition)
        {
            if (_isFirstPlay) return;
            _isFirstPlay = condition;
        }
    }
}