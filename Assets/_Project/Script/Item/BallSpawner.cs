using System;
using System.Collections.Generic;
using UnityEngine;
using Defend.Enum;

using Random = UnityEngine.Random;

namespace Defend.Item
{
    public class BallSpawner : MonoBehaviour
    {
        [Serializable]
        private struct BallPool
        {
            public string Key;
            public float Percentage;
            public Ball Prefabs;
        }

        #region Fields & Properties

        [Header("Pooler")]
        [SerializeField] private BallPool[] poolDatas;
        [SerializeField] private int poolCapacity;
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
                    for (var j = 0; j < poolCapacity; j++)
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
        public void SpawnBall()
        {
            var ball = GetBall(GetRandomPool());

            SpawnedBall = ball;
            ball.gameObject.SetActive(true);
            ball.transform.position = spawnPoints.position;
        }

        // !- Helpers
        public bool IsSuperBall()
        {
            var ball = SpawnedBall;
            return ball.BallType == BallType.Super && ball.gameObject.activeSelf;
        }

        private BallPool GetRandomPool()
        {
            var randomValue = Random.value;
            var cumulativePercent = 0f;

            foreach (var data in poolDatas)
            {
                cumulativePercent += data.Percentage / 100f;
                if (randomValue <= cumulativePercent)
                {
                    return data;
                }
            }

            return poolDatas[^1];
        }

        #endregion
    }
}
