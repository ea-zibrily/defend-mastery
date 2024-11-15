using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Defend.Redundant
{
    public class BallAirTween : MonoBehaviour
    {
        [SerializeField] private float jumpTime;
        [SerializeField] private float jumpPower;
        [SerializeField] private Transform targetPoint;
        [SerializeField] private Transform dropPoint;

        private Vector3 _originPoint;

        private void Start()
        {
            _originPoint = transform.position;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("jump!");
                BounceBall();
            }
        }
        
        private void BounceBall()
        {
            Sequence bounceSequence = DOTween.Sequence();

            bounceSequence.Append(transform.DOJump(targetPoint.position, jumpPower, 1, jumpTime, snapping: false)
                    .SetEase(Ease.Linear));
            
            bounceSequence.Append(transform.DOMove(dropPoint.position, jumpTime, snapping: false));
            
            bounceSequence.OnComplete(() =>
            {
                Debug.Log("done!");
                transform.position = _originPoint;
            });
        }
    }
}
