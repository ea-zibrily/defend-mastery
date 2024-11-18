using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Defend.Redundant
{
    public class BallBounceTween : MonoBehaviour
    {
        [Header("Single")]
        [SerializeField] private float jumpTime;
        [SerializeField] private int jumpNum;
        [SerializeField] private float jumpPower;
        [SerializeField] private Vector3[] wayPoints;
        
        private Tween _currentTween;
        private Vector3 _originPoint;

        private void Start()
        {
            _originPoint = transform.position;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("jump!");
                BounceBall();
            }
        }
        
        public void BounceBall()
        {
            Sequence bounceSequence = DOTween.Sequence();

            bounceSequence.Append(transform.DOJump(wayPoints[0], jumpPower, jumpNum, 0.5f, snapping: false)
                    .SetEase(Ease.Linear));
            bounceSequence.Append(transform.DOJump(wayPoints[1], 2.45f, jumpNum, jumpTime, snapping: false)
                    .SetEase(Ease.Linear));
            
            bounceSequence.OnComplete(() =>
            {
                Debug.Log("done bounce!");
                transform.position = _originPoint;
            });

            _currentTween = bounceSequence;
        }

        public void ReboundBall()
        {
            _currentTween?.Kill();
            transform.DOJump(_originPoint, 3, 1, 1.3f, snapping: false).SetEase(Ease.Linear);
        }
    }
}