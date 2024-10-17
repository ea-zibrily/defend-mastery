using System;
using Defend.Enum;
using UnityEngine;

namespace Defend.Item
{
    [CreateAssetMenu(fileName = "NewBallData", menuName = "Data/Ball Data", order = 0)]
    public class BallData : ScriptableObject
    {
        [Serializable]
        public struct PointData
        {
            public float[] ScorePoints;
            public float[] HealthPoints;
        }

        [Header("Data")]
        [SerializeField] private string ballName;
        [SerializeField] private BallType ballType;
        [SerializeField] private PointData ballPoint;

        // Getter
        public string BallName => ballName;
        public BallType BallType => ballType;
        public PointData BallPoint => ballPoint;
    }
}