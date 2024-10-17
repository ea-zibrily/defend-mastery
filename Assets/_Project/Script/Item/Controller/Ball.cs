using UnityEngine;
using Defend.Enum;
using Defend.Managers;

namespace Defend.Item
{
    public class Ball : MonoBehaviour
    {
        #region Fields & Properties

        [Header("General")]
        [SerializeField] private string ballName;
        [SerializeField] protected BallType ballType;

        [Header("Stats")]
        [SerializeField] private float ballSpeed;
        [SerializeField] protected float ballRotation;
        [SerializeField] private float ballLimiter;
        [SerializeField] protected bool canMove;

        protected float _currentRotation;
        public BallType Type => ballType;

        public float CurrentSpeed { get; set;}
        public bool CanMove
        {
            get => canMove;
            set => canMove = value;
        }

        // Reference
        protected SpriteRenderer ballSr;
        public BallSpawner BallSpawner { get; set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            InitOnAwake();
        }

        private void OnEnable()
        {
            InitOnEnable();
        }

        private void Update()
        {
            if (!GameManager.IsGameRunning) return;

            // Rotate
            ballSr.transform.Rotate(Vector3.forward * _currentRotation);

            // Move
            if (!CanMove) return;
            transform.Translate(CurrentSpeed * Time.deltaTime * Vector2.left);
            if (transform.position.x <= ballLimiter)
            {
                canMove = false;
                BallSpawner.ReleaseBall(this); 
            }
        }

        #endregion

        #region Methods

        // !- Initialize
        protected virtual void InitOnAwake() 
        {
            ballSr = GetComponentInChildren<SpriteRenderer>();
        }
    
        protected virtual void InitOnEnable()
        {
            gameObject.name = ballName;

            CurrentSpeed = ballSpeed;
            _currentRotation = ballRotation;
        }

        // !- Core
        public virtual void Deflect() { }
        public virtual void Undeflect() { }
        
        #endregion
    }
}