using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace Defend.Animation
{
    public class ParabolicAnimation : MonoBehaviour
    {
        #region Fields & Property

        [Header("Stats")]
        [SerializeField] private Ease easeType;
        [SerializeField] private PathType pathType = PathType.CatmullRom;
        [SerializeField] private float duration;
        [SerializeField] private Vector3[] waypointMultipliers;

        private List<Vector3> _waypoints;
        private Tween _ballTween;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            // Init tween
            DOTween.Init(true, false, LogBehaviour.Verbose).SetCapacity(200, 10);

            // Init waypoint
            var wayLenght = waypointMultipliers.Length;
            _waypoints ??= new List<Vector3>(wayLenght);
            for (var i = 0; i < wayLenght; i++)
            {
                _waypoints.Add(Vector3.zero);
            }
        }

        #endregion

        #region Methods

        // !- Core
        private void InitWaypoints(Transform target)
        {
            if (target == null) return;
            for (var i = 0; i < _waypoints.Count; i++)
            {
                var posTarget = target.position;
                _waypoints[i] = posTarget + waypointMultipliers[i];
            }
        }

        public void AnimateBall(Transform target, TweenCallback callback = null)
        {
            InitWaypoints(target);
            DoPath(callback);
        }

        private void DoPath(TweenCallback callback = null)
        {
            _ballTween?.Kill(false);
            _ballTween = transform.DOPath(_waypoints.ToArray(), duration, pathType).SetEase(easeType);
            _ballTween.onComplete += callback;
        }

        #endregion
    }
}
