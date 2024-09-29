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
            ballAnimation.AnimateBall(transform, () =>
            {
                BallSpawner.ReleaseBall(this);
            });
        }

        #endregion
    }
}