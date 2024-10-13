using System.Collections.Generic;
using UnityEngine;
using Defend.Enum;
using Defend.Item;
using Defend.Managers;
using Defend.Animation;

namespace Defend.Gameplay
{
    public class DeflectHandler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Deflect")]
        [SerializeField] private List<Ball> availableBalls;
        [SerializeField]  private bool canBeHold;

        private Ball _targetBall;
        // private bool _canBePressed;

        [Header("Reference")]
        [SerializeField] private CharacterAnimation characterAnim;


        #endregion
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            canBeHold = false;
            availableBalls = new List<Ball>();
            // _canBePressed = false;
        }

        private void Update()
        {
            if (!GameManager.IsGameRunning) return;

            HandleBallDeflect();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Ball>(out var ball))
            {
                AddAvailableBall(ball);
                // _canBePressed = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Ball>(out var ball))
            {
                RemoveAvailableBall(ball);
                // _canBePressed = false;
            }
        }
        
        #endregion

        #region Methods
        
        private void HandleBallDeflect()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Animation
                characterAnim.SetAnimation(CharacterState.Idle);

                // Deflect
                _targetBall = GetNearestBall();
                if (_targetBall != null)
                {
                    canBeHold = _targetBall.BallType == BallType.Super;
                    if (!canBeHold)
                    {
                        _targetBall.Deflect();
                        RemoveAvailableBall(_targetBall);
                        _targetBall = null;

                        // Check bom type
                        if (_targetBall.BallType == BallType.Bom)
                            characterAnim.SetAnimation(CharacterState.Boom);
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (_targetBall != null && canBeHold)
                {
                    _targetBall.Deflect();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // Animation
                characterAnim.SetAnimation(CharacterState.Idle);

                // Undeflect
                if (_targetBall != null && canBeHold)
                {
                    _targetBall.Undeflect();
                    RemoveAvailableBall(_targetBall);
                    _targetBall = null;
                }
                canBeHold = false;
            }
        }

        // private void HandleDeflectOld()
        // {
        //     if (_availableBalls.Count < 1) return;

        //     var currentBall = GetLatestBall();
        //     if (currentBall.BallType == BallType.Super)
        //     {
        //         if (Input.GetMouseButton(0) && _canBePressed)
        //         {
        //             currentBall.Deflect();
        //         }
        //         else if (Input.GetMouseButtonUp(0))
        //         {
        //             currentBall.Undeflect();
        //             _canBePressed = false;
        //         }
        //     }
        //     else
        //     {
        //         if (Input.GetMouseButtonDown(0) && _canBePressed)
        //         {
        //             currentBall.Deflect();
        //             _canBePressed = false;
        //         }
        //     }
        // }

        private Ball GetNearestBall()
        {
            if (availableBalls == null || availableBalls.Count == 0) return null;

            Ball targetBall = null;
            float nearest = float.MaxValue;

            foreach (var ball in availableBalls)
            {
                var distance = Vector2.Distance(transform.position, ball.transform.position);
                if (distance < nearest)
                {
                    nearest = distance;
                    targetBall = ball;
                }
            }

            return targetBall;
        }


        private void AddAvailableBall(Ball ball)
        {
            if (!availableBalls.Contains(ball))
            {
                availableBalls.Add(ball);
            }
        }
        private void RemoveAvailableBall(Ball ball)
        {
            if (availableBalls.Contains(ball))
            {
                availableBalls.Remove(ball);
            }
        }

        #endregion
    }
}
