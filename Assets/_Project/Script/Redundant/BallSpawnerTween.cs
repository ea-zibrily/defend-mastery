using System.Collections;
using System.Collections.Generic;
using Defend.Redundant;
using UnityEngine;

namespace Defend
{
    public class BallSpawnerTween : MonoBehaviour
    {
        [SerializeField] private int spawnNum;
        [SerializeField] private BallBounceTween ballTween;
        [SerializeField] private Transform spawnPosition;

        private void Start()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            for (int i = 0; i < spawnNum; i++)
            {
                var ball = Instantiate(ballTween, spawnPosition.position, Quaternion.identity);
                ball.transform.SetParent(transform);
                ball.BounceBall();
                
                yield return new WaitForSeconds(0.5f);
            }     
        }
    }
}
