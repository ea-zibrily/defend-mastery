using UnityEngine;
using Defend.Enum;
using Defend.Item;
using Defend.Managers;

namespace Defend
{
    public class DeflectHandler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private bool canBePressed;
        [SerializeField] private Ball _currentBall;

        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Update()
        {
            if (!GameManager.IsGameRunning) return;

            HandleDeflect();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                canBePressed = true;
                _currentBall = other.GetComponent<Ball>();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                canBePressed = false;
                _currentBall = null;
            }
        }

        #endregion

        #region Methods

        private void HandleDeflect()
        {
            if (_currentBall == null) return;

            var isSuperBall = _currentBall.BallType == BallType.Super;
            if (isSuperBall)
            {
                if (Input.GetMouseButton(0) && canBePressed)
                {
                    Debug.Log("deflect super ball");
                    _currentBall.Deflect();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Debug.LogWarning("undeflect super ball");
                    _currentBall.Undeflect();
                    canBePressed = false;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && canBePressed)
                {
                    _currentBall.Deflect();
                    canBePressed = false;
                }
            }
        }

        #endregion
    }
}
