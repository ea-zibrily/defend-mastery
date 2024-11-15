using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Defend.Redundant
{
    public class BallStraightTween : MonoBehaviour
    {
        [Header("Single")]
        [SerializeField] private float tweenTime;
        [SerializeField] private Ease easeType;
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
                transform.DOMove(targetPoint.position, tweenTime, snapping: false)
                    .SetEase(easeType)
                    .OnComplete(() => 
                        {
                            Debug.Log("done!");
                            transform.position = _originPoint;
                        });
            }    
        }
    }
}
