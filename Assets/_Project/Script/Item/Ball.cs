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
        [SerializeField] private float ballRotation;
        [SerializeField] protected bool canMove;
        [SerializeField] private Transform limitTransform;

        public BallType BallType => ballType;
        public bool CanMove => canMove;

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
            ballSr.transform.Rotate(Vector3.forward * ballRotation);

            // Move
            if (!canMove) return;
            transform.Translate(ballSpeed * Time.deltaTime * Vector2.left);
            if (Vector2.Distance(transform.position, limitTransform.position) < 0.01)
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
            canMove = true;
        }

        // !- Core
        public virtual void Deflect() { }
        public virtual void Undeflect() { }
        
        #endregion
    }
}