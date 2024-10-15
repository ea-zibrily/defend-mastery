using System.Collections.Generic;
using UnityEngine;
using Defend.Enum;
using Defend.Item;
using Defend.Managers;
using Defend.Animation;

namespace Defend.Gameplay
{
    public class DeflectController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Deflect")]
        [SerializeField] private List<Ball> availableBalls;
        [SerializeField] private CharacterAnimation characterAnim;

        private bool _canBeHold;
        private Ball _targetBall;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            _canBeHold = false;
            availableBalls = new List<Ball>();
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
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Ball>(out var ball))
            {
                RemoveAvailableBall(ball);
            }
        }
        
        #endregion

        #region Methods
        
        private void HandleBallDeflect()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _targetBall = GetNearestBall();

                // Animation
                var animState = GetStateByBall(_targetBall == null ? BallType.Normal : _targetBall.Type);
                characterAnim.SetAnimation(animState);

                // Deflect
                if (_targetBall != null)
                {
                    _canBeHold = _targetBall.Type == BallType.Super;
                    if (!_canBeHold)
                    {
                        _targetBall.Deflect();
                        RemoveAvailableBall(_targetBall);
                        _targetBall = null;
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (_targetBall != null && _canBeHold)
                {
                    _targetBall.Deflect();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // Animation
                characterAnim.SetAnimation(CharacterState.Idle);

                // Undeflect
                if (_targetBall != null && _canBeHold)
                {
                    _canBeHold = false;
                    _targetBall.Undeflect();
                    RemoveAvailableBall(_targetBall);
                    _targetBall = null;
                }
            }
        }

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

        private CharacterState GetStateByBall(BallType type)
        {
            return type switch
            {
                BallType.Normal => CharacterState.Deflect,
                BallType.Super => CharacterState.Super,
                BallType.Bom => CharacterState.Boom,
                _ => CharacterState.Idle
            };
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
