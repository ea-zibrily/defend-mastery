using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Defend.Enum;
using Defend.Item;
using Defend.Events;
using Defend.Managers;
using Defend.Animation;

namespace Defend.Gameplay
{
    public class DeflectController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Deflect")]
        [SerializeField] private float[] deflectAreas;
        [SerializeField] private float offArea;
        
        private bool _canBeHold;
        private Vector3 _offPosition;
        private Ball _targetBall;
        private readonly List<Ball> _availableBalls = new();

        [Header("Reference")]
        [SerializeField] private CharacterAnimation characterAnim;

        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Start()
        {
            _canBeHold = false;
            _offPosition = transform.position + new Vector3(offArea, 0, 0);
        }
        
        private void Update()
        {
            if (!GameManager.IsGameRunning) return;

            OffAreaChecker();
            HandleBallDeflect();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                var ball = other.GetComponent<Ball>();
                _availableBalls.Add(ball);
            }
        }
        
        #endregion
        
        #region Methods
        
        private void HandleBallDeflect()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _targetBall = GetNearestBall();

                // Deflect
                if (_targetBall != null)
                {
                    var status = GetStatusByDistance(_targetBall);
                    GameEvents.DeflectBallEvent(_targetBall, status);

                    // Animation
                    var tempType = status != DeflectStatus.Miss ? _targetBall.Type : BallType.Normal;
                    var animState = GetStateByBall(tempType);
                    characterAnim.SetAnimation(animState);

                    if (status != DeflectStatus.Miss)
                    {
                        _canBeHold = _targetBall.Type == BallType.Super;
                        if (!_canBeHold)
                        {                            
                            _targetBall.Deflect();
                            _availableBalls.Remove(_targetBall);
                            _targetBall = null;
                        }   
                    }
                    else
                    {
                        _availableBalls.Remove(_targetBall);
                        _targetBall = null;
                    }
                }
                else
                {
                    // Animation
                    characterAnim.SetAnimation(CharacterState.Deflect);
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
                    _availableBalls.Remove(_targetBall);
                    _targetBall = null;
                }
            }
        }
        
        private void OffAreaChecker()
        {
            if (_availableBalls == null) return;

            foreach (var ball in _availableBalls.ToList())
            {
                var ballPosition = ball.transform.position;
                if (ballPosition.x <= _offPosition.x)
                {
                    if (_availableBalls.Contains(ball))
                    {
                        GameEvents.DeflectBallEvent(ball, DeflectStatus.Miss);
                        _availableBalls.Remove(ball);
                    }
                }
            }
        }
        
        private Ball GetNearestBall()
        {
            if (_availableBalls == null || _availableBalls.Count == 0) return null;

            Ball targetBall = null;
            float nearest = float.MaxValue;

            foreach (var ball in _availableBalls)
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

            if (distance <= deflectAreas[0]) return DeflectStatus.Perfect;
            if (distance <= deflectAreas[1]) return DeflectStatus.Good;
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

        private void OnDrawGizmos()
        {
            // Area color
            Color offColor = Color.cyan;
            Color missColor = Color.red;
            Color goodColor = Color.yellow;
            Color perfectColor = Color.blue;

            var startPosition = transform.position;

            // Draw area
            Gizmos.color = offColor;
            Gizmos.DrawLine(startPosition, startPosition + new Vector3(offArea, 0, 0));

            Gizmos.color = missColor;
            Gizmos.DrawLine(startPosition, startPosition + new Vector3(deflectAreas[2], 0, 0));

            Gizmos.color = goodColor;
            Gizmos.DrawLine(startPosition, startPosition + new Vector3(deflectAreas[1], 0, 0));

            Gizmos.color = perfectColor;
            Gizmos.DrawLine(startPosition, startPosition + new Vector3(deflectAreas[0], 0, 0));
        }

        #endregion
    }
}
