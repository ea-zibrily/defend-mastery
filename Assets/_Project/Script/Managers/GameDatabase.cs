using Defend.Singleton;

namespace Defend.Managers
{
    public class GameDatabase : MonoDDOL<GameDatabase>
    {
        // Fields
        private bool _isFirstPlay;
        public bool IsFirstPlay => _isFirstPlay;

        public void SetFirstPlay(bool condition)
        {
            if (_isFirstPlay) return;
            _isFirstPlay = condition;
        }

    }
}