using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Defend
{
    public class BallReboundTween : MonoBehaviour
    {
        [SerializeField] private float jumpTime;
        [SerializeField] private float jumpPower;
        [SerializeField] private Transform targetPoint;
        
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
                transform.DOJump(targetPoint.position, jumpPower, 1, jumpTime, snapping: false)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                        {
                            Debug.Log("done!");
                            transform.position = _originPoint;
                        });
            }
        }
    }
}
