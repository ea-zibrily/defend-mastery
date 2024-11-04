using System;
using UnityEngine;
using Defend.Enum;

namespace Defend.Item
{
    [CreateAssetMenu(fileName = "NewBallData", menuName = "Data/Ball Data", order = 0)]
    public class BallData : ScriptableObject
    {
        [Serializable]
        public struct PointData
        {
            public DeflectStatus Status;
            public float Point;
        }

        [Header("Data")]
        [SerializeField] private string ballName;
        [SerializeField] private BallType ballType;
        [SerializeField] private PointData[] scorePoints;
        [SerializeField] private PointData[] healthPoints;

        // Getter
        public string BallName => ballName;
        public BallType BallType => ballType;
        public float GetScorePoint(DeflectStatus status)
        {
            PointData tempData = Array.Find(scorePoints, data => data.Status == status);
            
            // Check available data
            if (tempData.Equals(default(PointData)))
                return 0f;

            return tempData.Point;
        }

        public float GetHealthPoint(DeflectStatus status)
        {
            PointData tempData = Array.Find(healthPoints, data => data.Status == status);
            
            // Check available data
            if (tempData.Equals(default(PointData)))
                return 0f;

            return tempData.Point;
        }

        // public float[] ScorePoints => scorePoints;
        // public float[] HealthPoints => healthPoints;
    }
}