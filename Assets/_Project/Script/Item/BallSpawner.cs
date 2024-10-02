using System;
using System.Collections.Generic;
using UnityEngine;
using Defend.Enum;

namespace Defend.Item
{
    public class BallSpawner : MonoBehaviour
    {
        [Serializable]
        private struct BallPool
        {
            public string Key;
            public float Capacity;
            public Ball Prefabs;
        }

        #region Fields & Properties

        [Header("Pooler")]
        [SerializeField] private BallPool[] poolDatas;
        [SerializeField] private Transform poolParent;
        [SerializeField] protected Transform spawnPoints;

        public Ball SpawnedBall { get; private set; }
        private readonly Dictionary<string, List<Ball>> poolDictionary = new();

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            // Init pooler
            var dataLenght = poolDatas.Length;
            for (var i = 0; i < dataLenght; i++)
            {
                var data = poolDatas[i];
                if (!poolDictionary.ContainsKey(data.Key))
                {
                    poolDictionary.Add(data.Key, new List<Ball>());
                    for (var j = 0; j < data.Capacity; j++)
                    {
                        var Ball = CreateBall(data.Prefabs);
                        poolDictionary[data.Key].Add(Ball);
                    }
                }
            }
        }

        #endregion

        #region Methods

        // !- Pooler
        private Ball CreateBall(Ball prefabs)
        {
            Ball Ball = Instantiate(prefabs, transform.position, Quaternion.identity);
            Ball.transform.parent = poolParent;
            Ball.gameObject.SetActive(false);
                        
            // Inject
            if (Ball.BallSpawner == null)
                Ball.BallSpawner = this;
            
            return Ball;
        }

        private Ball GetBall(BallPool data)
        {
            var key = data.Key;
            if (poolDictionary.ContainsKey(key))
            {
                // Check avail Ball
                List<Ball> BallPool = poolDictionary[key];
                for (var i = 0; i < BallPool.Count; i++)
                {
                    var Ball = BallPool[i];
                    if (!Ball.gameObject.activeInHierarchy)
                    {
                        Ball.transform.position = transform.position;
                        return Ball;
                    }
                }

                // Create new Ball
                Ball newBall = CreateBall(data.Prefabs);
                poolDictionary[key].Add(newBall);
                return newBall;
            }
            return null;
        }

        public void ReleaseBall(Ball Ball)
        {
            Ball.gameObject.SetActive(false);
        }

        // !- Core
        public void SpawnBall(BallType ballType, float duration = 0)
        {
            var ball = GetBall( GetPoolByType(ballType));
            SpawnedBall = ball;

            ball.gameObject.SetActive(true);
            ball.transform.position = spawnPoints.position;
            ball.CanMove = true;
            if (ball is SuperBall superBall)
                superBall.DeflectDuration = duration;
        }

        // !- Helpers
        private BallPool GetPoolByType(BallType ballType)
        {
            foreach (var data in poolDatas)
            {
                if (data.Key != ballType.ToString()) continue;
                return data;
            }

            return poolDatas[^1];
        }

        #endregion
    }
}
