using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Defend
{
    public class BlendAnimation : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.DOBlendableMoveBy(new Vector3(3, 3, 0), 3);
                transform.DOBlendableMoveBy(new Vector3(-3, 0, 0), 1f).SetLoops(3, LoopType.Yoyo);
            }
        }
    }
}
