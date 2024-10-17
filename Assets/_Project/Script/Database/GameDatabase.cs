using Defend.Singleton;

namespace Defend.Database
{
    public class GameDatabase : MonoDDOL<GameDatabase>
    {
        private bool _isFirstPlay;
        public bool IsFirstPlay => _isFirstPlay;

        public void SetFirstPlay(bool condition)
        {
            if (_isFirstPlay) return;
            _isFirstPlay = condition;
        }
    }
}