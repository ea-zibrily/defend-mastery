using System;
using UnityEngine;
using Defend.Enum;

namespace Defend.Animation
{
    public class CharacterAnimation : MonoBehaviour
    {
        
        #region Fields & Properties
        
        private int _currentState;
        private Animator _characterAnim;

        // Cached fields
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Deflect = Animator.StringToHash("Deflect");
        private static readonly int Boom = Animator.StringToHash("Boom");

        // Cached fields - Spine
        // public const string IDLE = "idle";
        // public const string DEFLECT = "deflect";
        // public const string BOOM = "boom";

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _characterAnim = GetComponent<Animator>();
        }

        private void Start()
        {
            var startState = CharacterState.Idle;
            SetAnimation(startState);
        }

        #endregion

        #region Methods

        public void SetAnimation(CharacterState target)
        {
            var state = GetState(target);

            if (state == _currentState) return;
            _characterAnim.CrossFade(state, 0, 0);
            _currentState = state;
        }
        
        private int GetState(CharacterState state)
        {
            return state switch
            {
                CharacterState.Idle => Idle,
                CharacterState.Deflect => Deflect,
                CharacterState.Boom => Boom,
                _ => throw new InvalidOperationException("Invalid character state"),
            };
        }

        #endregion
    }
}