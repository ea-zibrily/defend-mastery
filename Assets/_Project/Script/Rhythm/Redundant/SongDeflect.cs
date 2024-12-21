using UnityEngine;

namespace Defend.Rhythm
{
    public class SongDeflect : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                if (other.TryGetComponent<SongBall>(out var ball))
                {
                    var distance = Vector2.Distance(transform.position, other.transform.position);
                    if (distance <= 0.1f)
                    {
                        Debug.Log($"deflect {ball}");
                        ball.Rebound();
                        // Destroy(other.gameObject);
                    }
                }
            }
        }
    }
}