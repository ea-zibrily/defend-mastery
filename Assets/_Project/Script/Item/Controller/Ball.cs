using System;
using UnityEngine;
using DG.Tweening;
using Defend.Enum;

namespace Defend.Item
{
    public class Ball : MonoBehaviour
    {
        [Serializable]
        protected struct BallWay
        {
            public string Name;
            public float Time;
            public float Power;
            public Vector3[] WayValue;
        }

        #region Fields & Properties

        [Header("General")]
        [SerializeField] private string ballName;
        [SerializeField] protected BallType ballType;
        
        [Header("Stats")]
        [SerializeField] protected float rotationTime;
        [SerializeField] protected bool canSpawn = true;
        [SerializeField] protected Ease tweenEase;
        [SerializeField] protected BallWay[] ballWays;

        private float _rotateAngle;
        private Vector3 _originPoint;
        public BallType Type => ballType;
        public bool CanSpawn
        {
            get => canSpawn;
            set => canSpawn = value;
        }
        
        protected Tween moveTween;

        // Reference
        protected SpriteRenderer ballSr;
        public BallSpawner BallSpawner { get; set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            InitOnAwake();
        }

        private void OnEnable()
        {
            InitOnEnable();
        }

        private void Start()
        {
            gameObject.name = ballName;
            _originPoint = transform.position;
        }
        
        #endregion

        #region Methods

        // !- Initialize
        protected virtual void InitOnAwake() 
        {
            ballSr = GetComponentInChildren<SpriteRenderer>();
        }
        protected virtual void InitOnEnable()
        {
            _rotateAngle = 360f;
        }

        protected void ReleaseBall()
        {            
            CanSpawn = true;
            transform.position = _originPoint;
            BallSpawner.ReleaseBall(this);
        }
        
        // !- Core
        public virtual void Deflect() { }
        public virtual void Undeflect() { }
        public virtual void Move()
        {
            Rotate(isRight: false);
        }

        protected void Rotate(bool isRight)
        {
            if (isRight)
            {
                _rotateAngle *= -1;
            }

            ballSr.transform.DORotate(new Vector3(0f, 0f, _rotateAngle), rotationTime)
                .SetEase(Ease.Linear)
                .SetRelative()
                .SetLoops(-1, LoopType.Restart);
        }
        
        // Move type
        protected void Rebound()
        {
            // Rotate right
            Rotate(isRight: true);

            // Curve left
            var data = ballWays[0];
            moveTween?.Kill(false);
            transform.DOJump(data.WayValue[0], data.Power, 1, data.Time, snapping: false)
                    .SetEase(tweenEase)
                    .OnComplete(ReleaseBall);
        }
        
        protected void Straight()
        {
            var ways = ballWays[1];
            moveTween = transform.DOMove(ways.WayValue[0], ways.Time, snapping: false)
                    .SetEase(tweenEase)
                    .OnComplete(ReleaseBall);
        }
        
        protected void Curve()
        {
            var data = ballWays[2];
            Sequence bounceSequence = DOTween.Sequence();

            bounceSequence.Append(transform.DOJump(data.WayValue[0], data.Power, 1, data.Time, snapping: false)
                    .SetEase(tweenEase));
            bounceSequence.Append(transform.DOMove(data.WayValue[1], data.Time, snapping: false));

            bounceSequence.OnComplete(ReleaseBall);
            
            // Set tween
            moveTween = bounceSequence;
        }
        
        #endregion
    }
}