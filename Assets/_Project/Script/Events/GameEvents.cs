using System;
using Defend.Enum;
using Defend.Item;

namespace Defend.Events
{
    public static class GameEvents
    {
        // Events
        public static event Action OnGameStart;
        public static event Action OnGameEnd;

        public static event Action<Ball, DeflectStatus> OnDeflectBall;
        
        // Caller
        public static void GameStartEvent() => OnGameStart?.Invoke();
        public static void GameEndEvent() => OnGameEnd?.Invoke();

        public static void DeflectBallEvent(Ball ball, DeflectStatus status) => OnDeflectBall?.Invoke(ball, status);
    }
}