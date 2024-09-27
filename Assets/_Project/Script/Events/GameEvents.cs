using System;

namespace Defend.Events
{
    public static class GameEvents
    {
        // Events
        public static event Action OnGameStart;
        public static event Action OnGameEnd;
        
        // Caller
        public static void GameStartEvent() => OnGameStart?.Invoke();
        public static void GameEndEvent() => OnGameEnd?.Invoke();
    }
}