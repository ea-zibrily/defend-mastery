using UnityEngine;

namespace Defend.Item
{
    public class SuperBall : Ball
    {
        #region Internal Fields

        [Header("Super Ball")]
        [SerializeField] private float deflectDuration;
        [SerializeField] private GameObject ballEffect;
        
        public float DeflectDuration
        {
            get => deflectDuration;
            set => deflectDuration = value;
        }

        private float _currentTime;
        private readonly float normalRotation = 2f;

        #endregion

        #region Methods

        protected override void InitOnEnable()
        {
            base.InitOnEnable();
            _currentTime = 0f;
            ballSr.color = Color.red;
        }

        public override void Deflect()
        {
            base.Deflect();
            
            CanMove = false;
            _currentTime += Time.deltaTime;
            _currentRotation = Mathf.Lerp(ballRotation, normalRotation, _currentTime / DeflectDuration);
            ballSr.color = Color.Lerp(Color.red, Color.white, _currentTime / DeflectDuration);
            if (_currentTime >= DeflectDuration)
            {
                _currentTime = 0f;
                BallSpawner.ReleaseBall(this);
                // ballAnimation.AnimateBall(transform, () =>
                // {
                //     BallSpawner.ReleaseBall(this);
                // });
            }
        }

        public override void Undeflect()
        {
            base.Undeflect();
            CanMove = true;
        }

        #endregion
    }
}