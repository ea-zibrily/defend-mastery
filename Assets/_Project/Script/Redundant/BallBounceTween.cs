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
        [SerializeField] private Ease easeType;
        [SerializeField] private Transform dropPoint;
        [SerializeField] private Transform targetPoint;
        
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
                transform.DOJump(targetPoint.position, jumpPower, jumpNum, jumpTime, snapping: false)
                    .SetEase(easeType)
                    .OnComplete(() =>
                    {
                        Debug.Log("done!");
                        transform.position = _originPoint;
                    });
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("jump!");
                BounceBall();
            }
        }

        private void BounceBall()
        {
            Sequence bounceSequence = DOTween.Sequence();

            bounceSequence.Append(transform.DOJump(dropPoint.position, jumpPower, jumpNum, 0.5f, snapping: false)
                    .SetEase(Ease.Linear));
            bounceSequence.Append(transform.DOJump(targetPoint.position, 2.45f, jumpNum, jumpTime, snapping: false)
                    .SetEase(Ease.Linear));

            bounceSequence.OnComplete(() =>
            {
                Debug.Log("done!");
                transform.position = _originPoint;
            });
        }
    }
}