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

        public override void Deflect()
        {
            base.Deflect();

            canMove = false;
            // TODO: Drop mekanik bom meledug
            BallSpawner.ReleaseBall(this); 
        }

        #endregion
    }
}