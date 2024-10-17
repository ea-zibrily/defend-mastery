using System.Collections.Generic;
using UnityEngine;
using Defend.Enum;
using Defend.Item;
using Defend.Managers;
using Defend.Animation;
using System;
using Defend.Events;

namespace Defend.Gameplay
{
    public class DeflectController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Deflect")]
        [SerializeField] private List<Ball> availableBalls;
        
        private bool _canBeHold;
        private Ball _targetBall;
        public List<Ball> AvailableBalls => availableBalls;

        [Header("Reference")]
        [SerializeField] private CharacterAnimation characterAnim;
        private InnerAreaHandler _innerArea;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _innerArea = GetComponentInChildren<InnerAreaHandler>();
        }
        
        private void Start()
        {
            _canBeHold = false;
            _innerArea.Deflect = this;
            availableBalls = new List<Ball>();
        }

        private void Update()
        {
            if (!GameManager.IsGameRunning) return;
            HandleBallDeflect();
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
                    if (_innerArea.IsOnInnerArea)
                    {
                        // Store status by distance
                        var status = GetStatusByDistance(_targetBall);
                        GameEvents.DeflectBallEvent(_targetBall, status);

                        _canBeHold = _targetBall.Type == BallType.Super;
                        if (!_canBeHold)
                        {
                            _innerArea.IsOnInnerArea = false;
                            
                            _targetBall.Deflect();
                            availableBalls.Remove(_targetBall);
                            _targetBall = null;
                        }   
                    }
                    else
                    {
                        // Store miss status
                        GameEvents.DeflectBallEvent(_targetBall, DeflectStatus.Miss);
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
                    _innerArea.IsOnInnerArea = false;

                    _targetBall.Undeflect();
                    availableBalls.Remove(_targetBall);
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

        private DeflectStatus GetStatusByDistance(Ball ball)
        {
            var distance = Vector2.Distance(transform.position, ball.transform.position);
            Debug.Log(distance);

            if (distance <= 1.5f) return DeflectStatus.Perfect;
            if (distance <= 2.5f) return DeflectStatus.Good;
            return DeflectStatus.Miss;
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

        #endregion
    }
}
