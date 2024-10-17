using UnityEngine;

namespace Defend.Gameplay
{
    public class InnerAreaHandler : MonoBehaviour
    {
        // Inner
        public bool IsOnInnerArea { get; set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                IsOnInnerArea = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                IsOnInnerArea = false;
            }
        }
    }
}