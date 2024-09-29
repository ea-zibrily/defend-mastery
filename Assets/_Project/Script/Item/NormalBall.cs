namespace Defend.Item
{
    public class NormalBall : Ball
    {
        #region Methods

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