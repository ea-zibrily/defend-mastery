using UnityEngine;
using DG.Tweening;

namespace Defend.Item
{
    public class SuperBall : Ball
    {
        #region Internal Fields

        [Header("Super")]
        [SerializeField] private GameObject ballEffect;
        
        private float _currentTime;
        public float DeflectTime { get; set;}

        #endregion

        #region Methods

        protected override void InitOnEnable()
        {
            base.InitOnEnable();

            _currentTime = 0f;
            ballSr.color = Color.red;
        }

        public override void Move()
        {
            base.Move();
            Straight();
        }

        public override void Deflect()
        {
            base.Deflect();

            if (!moveTween.IsActive()) return;

            moveTween.Pause();
            _currentTime += Time.deltaTime;
            ballSr.color = Color.Lerp(Color.red, Color.white, _currentTime / DeflectTime);
            
            if (_currentTime >= DeflectTime)
            {
                _currentTime = DeflectTime;
                Rebound();
            }
        }
        
        public override void Undeflect()
        {
            base.Undeflect();

            if (!moveTween.IsActive()) return;
            moveTween.Play();
        }

        #endregion
    }
}