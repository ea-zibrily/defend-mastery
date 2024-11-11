using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Defend.Animation
{
    public class BounceAnimation : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private Ease easeType;
        [SerializeField] private float pathDuration;
        [SerializeField] private float ballRotation;
        [SerializeField] private Transform[] wayPointTransforms;

        private SpriteRenderer _ballSr;

        private List<Vector3> _wayPoints;
        private Vector3 _originPoint;
        private Tween _ballTween;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _ballSr = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(650, 150);
        }
        
        private void Start()
        {
            _ballSr.transform.DORotate(new Vector3(0, 0, 360f), ballRotation)
                .SetLoops(-1, LoopType.Restart)
                .SetRelative()
                .SetEase(Ease.Linear);

            _originPoint = wayPointTransforms[0].position;
            transform.position = _originPoint;

            _wayPoints = new List<Vector3>();
            foreach (var point in wayPointTransforms)
            {
                var pointPosition = point.position;
                _wayPoints.Add(pointPosition);
            }
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.DOPath(_wayPoints.ToArray(), pathDuration, PathType.CatmullRom).
                    SetEase(easeType).OnComplete(() =>
                    {
                        Debug.Log("done animate!");
                        transform.position = _originPoint;
                    });
            }
        }

        #endregion
    }
}
