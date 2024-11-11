using UnityEngine;
using DG.Tweening;

public class BallBounce : MonoBehaviour
{
    // [Header("Default")]
    // [SerializeField] private Transform startPoint; // Starting point of the ball
    // [SerializeField] private Transform endPoint;   // Point where the ball will bounce up to
    // [SerializeField] private float bounceDepth = -3f; // How deep the ball dips for the bounce
    // [SerializeField] private float downTime = 0.5f;   // Duration of downward movement
    // [SerializeField] private float upTime = 0.4f;     // Duration of upward movement

    [Header("Newest")]
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float moveDuration;

    private Sequence _bounceSequence;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Bounce();
        }
    }

    private void Bounce()
    {
        var bounceSequence = DOTween.Sequence();

        bounceSequence.Append(transform.DOMove(wayPoints[0].position, moveDuration).SetEase(Ease.InQuad));
        bounceSequence.Append(transform.DOMove(wayPoints[1].position, moveDuration).SetEase(Ease.Linear));
        bounceSequence.Append(transform.DOMove(wayPoints[2].position, moveDuration).SetEase(Ease.Linear));
        bounceSequence.Append(transform.DOMove(wayPoints[3].position, moveDuration).SetEase(Ease.Linear));
        bounceSequence.Append(transform.DOMove(wayPoints[4].position, moveDuration).SetEase(Ease.OutQuad));

        bounceSequence.OnComplete(() =>
        {
            Debug.Log("bounce complete!");
            transform.position = wayPoints[0].position;
        });
    }
    
    // private void Bounce()
    // {
    //     // Reset position to start point
    //     transform.position = startPoint.position;

    //     // Calculate control points for the bounce path
    //     Vector3 midPoint = new Vector3(
    //         (startPoint.position.x + endPoint.position.x) / 2,
    //         startPoint.position.y + bounceDepth,
    //         startPoint.position.z
    //     );

    //     // Create a DOTween sequence
    //     Sequence bounceSequence = DOTween.Sequence();

    //     // Move down to the bounce depth
    //     bounceSequence.Append(transform.DOMove(midPoint, downTime).SetEase(Ease.InQuad));

    //     // Move up to the end point
    //     bounceSequence.Append(transform.DOMove(endPoint.position, upTime).SetEase(Ease.OutQuad));

    //     // Optional: Loop the bounce indefinitely
    //     bounceSequence.SetLoops(-1, LoopType.Restart);
    // }
}
