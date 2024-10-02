using UnityEngine;

namespace Defend.Rhythm
{
    public class SongBall : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private float ballSpeed = 7f;
        [SerializeField] protected float ballRotation = 2.5f;
        [SerializeField] private float ballLimiter ;
        [SerializeField] protected bool canMove;

        public float BallSpeed
        {
            get => ballSpeed;
            set => ballSpeed = value;
        }

        // Reference
        protected SpriteRenderer ballSr;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            ballSr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            canMove = true;
        }

        private void Update()
        {
            // Rotate
            ballSr.transform.Rotate(Vector3.forward * ballRotation);

            // Move
            transform.Translate(BallSpeed * Time.deltaTime * Vector2.left);
            if (transform.position.x <= ballLimiter)
            {
                canMove = false;
                Destroy(this);
            }
        }

        #endregion
    
    }
}