using UnityEngine;
using Defend.Item;

namespace Defend.Gameplay
{
    public class OuterAreaHandler : MonoBehaviour
    {
        // Reference
        private DeflectController _deflect;

        private void Awake()
        {
            _deflect = GetComponentInParent<DeflectController>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                var ball = other.GetComponent<Ball>();
                _deflect.AddAvailableBall(ball);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                var ball = other.GetComponent<Ball>();
                _deflect.AddAvailableBall(ball);
            }
        }
    }
}