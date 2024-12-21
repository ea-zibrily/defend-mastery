using UnityEngine;
using DG.Tweening; 

namespace Defend.Rhythm
{
    public class SongBall : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private float moveTime;
        [SerializeField] private float reboundTime;
        [SerializeField] private float reboundPower;
        [SerializeField] private float rotationTime;

        private float _rotateAngle;

        [Header("Tween")]
        [SerializeField] private Ease tweenEase;
        [SerializeField] private Vector3[] wayValue;

        private Tween _moveTween;
        private Tween _rotateTween;

        // Reference
        protected SpriteRenderer ballSr;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            ballSr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            _rotateAngle = 360f;
        }

        public void Rebound()
        {
            // Rotate(isRight: true);

            _moveTween?.Kill(false);
            transform.DOJump(wayValue[0], reboundPower, 1, reboundTime, snapping: false)
                    .SetEase(tweenEase)
                    .OnComplete(() => 
                    {
                        Destroy(gameObject);
                    });
        }
        
        public void Move()
        {
            // Rotate(isRight: false);
            _moveTween = transform.DOMove(wayValue[1], moveTime, snapping: false)
                    .SetEase(tweenEase)
                    .OnComplete(() => 
                    {
                        Destroy(gameObject);
                    });
        }

        protected void Rotate(bool isRight)
        {
            if (isRight)
            {
                _rotateAngle *= -1;
            }

            _rotateTween = ballSr.transform.DORotate(new Vector3(0f, 0f, _rotateAngle), rotationTime)
                .SetEase(Ease.Linear)
                .SetRelative()
                .SetLoops(-1, LoopType.Restart);
        }
        
        #endregion
    
    }
}