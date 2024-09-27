using UnityEngine;

namespace Defend.Item
{
    // TODO: Bikin mekanik hold bola
    public class SuperBall : Ball
    {
        #region Internal Fields

        [Header("Super Ball")]
        [SerializeField] private float deflectDuration;
        [SerializeField] private GameObject ballEffect;
        
        private float _currentTime;
        public bool IsSuperBallAppear;

        #endregion

        #region Methods

        protected override void InitOnEnable()
        {
            base.InitOnEnable();
            _currentTime = 0f;
        }

        public override void Deflect()
        {
            base.Deflect();

            canMove = false;
            _currentTime += Time.deltaTime;
            if (_currentTime >= deflectDuration)
            {
                // TODO: Drop mekanik bola melayang dan tleser
                BallSpawner.ReleaseBall(this); 
            }
        }

        public override void Undeflect()
        {
            base.Undeflect();
            canMove = true;
        }

        #endregion
    }
}