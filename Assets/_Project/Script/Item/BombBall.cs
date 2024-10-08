using Defend.Events;
using UnityEngine;

namespace Defend.Item
{
    public class BombBall : Ball
    {
        #region Internal Fields

        [Header("Bomb")]
        [SerializeField] private GameObject explodeEffect;

        #endregion

        #region Methods

        // TODO: Drop mekanik bom meledug
        public override void Deflect()
        {
            base.Deflect();
            CanMove = false;
            BallSpawner.ReleaseBall(this);
            GameEvents.GameEndEvent();
        }

        #endregion
    }
}