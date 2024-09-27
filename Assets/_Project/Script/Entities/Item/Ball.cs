using UnityEngine;
using Defend.Enum;

namespace Defend.Item
{
    public class Ball : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private string ballName;
        [SerializeField] protected BallType ballType;
        [SerializeField] private float ballSpeed;
        [SerializeField] private float ballRotation;
        [SerializeField] protected bool canMove;

        public BallType BallType => ballType;
        public bool CanMove => canMove;

        // Reference
        protected SpriteRenderer ballSr;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            InitOnAwake();
        }

        private void Start()
        {
            InitOnStart();
        }

        private void Update()
        {
            if (!canMove) return;
            MoveBall();
        }

        #endregion

        #region Methods

        // !- Initialize
        protected virtual void InitOnAwake() 
        {
            ballSr = GetComponentInChildren<SpriteRenderer>();
        }
    
        protected virtual void InitOnStart()
        {
            gameObject.name = ballName;
            canMove = true;
        }

        // !- Core
        public virtual void DeflectBall() { }
        protected void MoveBall()
        {
            transform.Translate(ballSpeed * Time.deltaTime * Vector2.left);
            ballSr.transform.Rotate(Vector3.forward * ballRotation);
        }
        
        #endregion
    }
}