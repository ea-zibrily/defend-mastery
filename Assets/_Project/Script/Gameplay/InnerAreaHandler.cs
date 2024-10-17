using Defend.Enum;
using Defend.Events;
using Defend.Item;
using UnityEngine;

namespace Defend.Gameplay
{
    public class InnerAreaHandler : MonoBehaviour
    {
        // Inner
        public bool IsOnInnerArea { get; set; }
        public DeflectController Deflect { get; set; }
        
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
                var ball = other.GetComponent<Ball>();
                if (Deflect.AvailableBalls.Contains(ball))
                {
                    GameEvents.DeflectBallEvent(ball, DeflectStatus.Miss);
                    Deflect.AvailableBalls.Remove(ball);
                }
            }
        }
    }
}