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

        #endregion

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