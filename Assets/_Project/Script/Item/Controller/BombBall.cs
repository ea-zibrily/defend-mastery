using UnityEngine;
using Defend.Events;

namespace Defend.Item
{
    public class BombBall : Ball
    {
        #region Internal Fields

        [Header("Bomb")]
        [SerializeField] private GameObject explodeEffect;
        private readonly int MaxWays = 3;

        #endregion

        #region Methods

        // TODO: Drop mekanik bom meledug
        public override void Deflect()
        {
            base.Deflect();
            
            CanSpawn = true;
            BallSpawner.ReleaseBall(this);
            GameEvents.GameEndEvent();
        }

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
            }
        }

        #endregion
    }
}