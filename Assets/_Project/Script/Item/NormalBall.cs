using UnityEngine;

namespace Defend.Item
{
    public class NormalBall : Ball
    {
        #region Methods

        public override void Deflect()
        {
            base.Deflect();
            
            CanMove = false;
            BallSpawner.ReleaseBall(this);
            // ballAnimation.AnimateBall(transform, () =>
            // {
            //     BallSpawner.ReleaseBall(this);
            // });
            // 
        }

        #endregion
    }
}