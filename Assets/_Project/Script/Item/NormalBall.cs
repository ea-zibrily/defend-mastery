namespace Defend.Item
{
    public class NormalBall : Ball
    {
        #region Methods

        public override void Deflect()
        {
            base.Deflect();
            
            canMove = false;
            // TODO: Bikin mekanik bola melayang keatas dan tleser kebawah
            BallSpawner.ReleaseBall(this); 
        }

        #endregion
    }
}