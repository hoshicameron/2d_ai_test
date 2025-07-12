using UnityEngine;

namespace PetalsOfHope.Gameplay.LevelElements
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector2 endPositionOffset;
        [SerializeField] private float speed = 2f;
        [SerializeField] private float waitTimeAtEnds = 1f;

        private Rigidbody2D rb;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private bool movingToEnd = true;
        private float waitTime;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            startPosition = rb.position;
            endPosition = startPosition + endPositionOffset;
        }

        private void FixedUpdate()
        {
            if (waitTime > 0)
            {
                waitTime -= Time.fixedDeltaTime;
                return;
            }

            var targetPosition = movingToEnd ? endPosition : startPosition;
            var newPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            if (!(Vector2.Distance(rb.position, targetPosition) < 0.01f)) return;
            waitTime = waitTimeAtEnds;
            movingToEnd = !movingToEnd;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            var currentStartPosition = Application.isPlaying ? startPosition : (Vector2)transform.position;
            var currentEndPosition = currentStartPosition + endPositionOffset;
            Gizmos.DrawLine(currentStartPosition, currentEndPosition);
            Gizmos.DrawWireSphere(currentStartPosition, 0.2f);
            Gizmos.DrawWireSphere(currentEndPosition, 0.2f);
        }
    }
}
