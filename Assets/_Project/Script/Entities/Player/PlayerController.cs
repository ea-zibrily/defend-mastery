using UnityEngine;
using Defend.Item;

namespace Defend.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Area")]
        [SerializeField] private float deflectRadius;
        [SerializeField] private Transform deflectTransform;
        [SerializeField] private LayerMask layerMask;

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            HandleDeflectArea();
        }

        #endregion

        #region Methods

        // !- Core
        private void HandleDeflectArea()
        {
            Collider2D area = Physics2D.OverlapCircle(deflectTransform.position, deflectRadius, layerMask);
            if (area != null)
            {
                var ball = area.GetComponent<Ball>();
                var isBombBall = ball.BallType == Enum.BallType.Bom;

                if (Input.GetMouseButtonDown(0) && !isBombBall)
                {
                    ball.DeflectBall();
                }
                else if (Input.GetMouseButton(0) && isBombBall)
                {
                    ball.DeflectBall();
                }
            }
        }

        // private void HandleScore()
        // {
        //     var distance = Vector2.Distance(deflectTransform.position, ball.transform.position);
        //     Debug.Log($"tap ball with {distance}");
        // }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(deflectTransform.position, deflectRadius);
        }

        #endregion
    }

}