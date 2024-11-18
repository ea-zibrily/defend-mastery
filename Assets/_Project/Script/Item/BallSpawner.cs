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
                        var ball = CreateBall(data.Prefabs);
                        poolDictionary[data.Key].Add(ball);
                    }
                }
            }
        }

        #endregion

        #region Methods

        // !- Pooler
        private Ball CreateBall(Ball prefabs)
        {
            Ball ball = Instantiate(prefabs, transform.position, Quaternion.identity);
            ball.transform.parent = poolParent;
            ball.gameObject.SetActive(false);
                        
            // Inject
            if (ball.BallSpawner == null)
                ball.BallSpawner = this;
            
            return ball;
        }

        private Ball GetBall(BallPool data)
        {
            var key = data.Key;
            if (poolDictionary.ContainsKey(key))
            {
                // Check avail Ball
                List<Ball> ballPool = poolDictionary[key];
                for (var i = 0; i < ballPool.Count; i++)
                {
                    var ball = ballPool[i];
                    if (!ball.gameObject.activeInHierarchy)
                    {
                        ball.transform.position = transform.position;
                        return ball;
                    }
                }

                // Create new Ball
                Ball newBall = CreateBall(data.Prefabs);
                poolDictionary[key].Add(newBall);
                return newBall;
            }
            return null;
        }
        
        public void ReleaseBall(Ball ball)
        {
            ball.gameObject.SetActive(false);
        }
        
        // !- Core
        public void SpawnBall(BallType ballType, float duration)
        {
            var ball = GetBall(GetPoolByType(ballType));
            
            ball.gameObject.SetActive(true);
            ball.transform.position = spawnPoints.position;
            ball.CanSpawn = false;
            ball.Move();

            if (ball is SuperBall superBall)
                superBall.DeflectTime = duration;
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
