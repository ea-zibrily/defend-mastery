using UnityEngine;
using DG.Tweening;

namespace Defend.Item
{
    public class NormalBall : Ball
    {
        #region Internal Fields

        [Header("Normal")]
        [SerializeField] private float downTime = 0.5f;
        [SerializeField] private float upPower = 2.45f;
        private readonly int MaxWays = 4;

        #endregion

        #region Methods

        public override void Move()
        {
            base.Move();

            if (ballWays.Length > MaxWays)
            {
                Debug.LogError("ball ways kebanyakan, maksimal 3!");
                return;
            };

            var randomIndex = Random.Range(1, ballWays.Length);
            switch (randomIndex)
            {
                case 1:
                    Straight();
                    break;
                case 2:
                    Curve();
                    break;
                case 3:
                    Bounce();
                    break;
            }
        }
        
        public override void Deflect()
        {
            base.Deflect();
            Rebound();
        }

        private void Bounce()
        {
            var ways = ballWays[3];
            Sequence bounceSequence = DOTween.Sequence();

            bounceSequence.Append(transform.DOJump(ways.WayValue[0], ways.Power, 1, downTime, snapping: false)
                    .SetEase(Ease.Linear));
            bounceSequence.Append(transform.DOJump(ways.WayValue[1], upPower, 1, ways.Time, snapping: false)
                    .SetEase(Ease.Linear));

            bounceSequence.OnComplete(ReleaseBall);

            // Set tween
            moveTween = bounceSequence;
        }

        #endregion
    }
}