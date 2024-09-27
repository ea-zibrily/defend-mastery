using UnityEngine;

namespace Defend.Item
{
    public class NormalBall : Ball
    {
        #region Methods

        // TODO: Bikin mekanik bola melayang keatas dan tleser kebawah
        public override void DeflectBall()
        {
            base.DeflectBall();
            canMove = false;
            gameObject.SetActive(false);
        }

        #endregion
    }
}