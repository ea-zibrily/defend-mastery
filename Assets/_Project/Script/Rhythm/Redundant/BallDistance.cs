using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defend
{
    public class BallDistance : MonoBehaviour
    {
        public bool canPress;
        public float dist;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("ball enter");
            canPress = true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log("ball stay");
            dist = Vector2.Distance(transform.position, other.transform.position);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("ball exit");
            canPress = false;
        }
    }
}
