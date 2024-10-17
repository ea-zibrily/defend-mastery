using System;
using UnityEngine;
using Defend.Item;
using Defend.Enum;
using Defend.Singleton;

namespace Defend.Database
{
    public class ScoreDatabase : MonoDDOL<ScoreDatabase>
    {
        [Header("Data")]
        [SerializeField] private BallData[] ballDatas;

        public BallData GetDataByType(BallType type)
        {
            BallData data = Array.Find(ballDatas, ball => ball.BallType == type);
            if (data != null) return data;
            
            return null;
        }
    }
}