using UnityEngine;

namespace Defend.Rhythm
{
    public class SongDeflect : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                var distance = Vector2.Distance(transform.position, other.transform.position);
                if (distance <= 0.1f)
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}