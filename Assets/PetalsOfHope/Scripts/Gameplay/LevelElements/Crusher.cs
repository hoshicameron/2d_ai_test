using UnityEngine;
using System.Collections;
using PetalsOfHope.Interfaces;

namespace PetalsOfHope.Gameplay.LevelElements
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Crusher : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Vector2 endPositionOffset;
        [SerializeField] private float crushSpeed = 10f;
        [SerializeField] private float returnSpeed = 2f;
        [SerializeField] private float delayAtTop = 2f;
        [SerializeField] private float delayAtBottom = 1f;

        [Header("Damage")]
        [SerializeField] private int damage = 9999; // Lethal by default

        private Rigidbody2D _rb;
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private bool _isCrushing;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        private void Start()
        {
            _startPosition = _rb.position;
            _endPosition = _startPosition + endPositionOffset;
            StartCoroutine(CrushCycle());
        }

        private IEnumerator CrushCycle()
        {
            while (true)
            {
                // Wait at the top
                yield return new WaitForSeconds(delayAtTop);

                // Crush down
                _isCrushing = true;
                yield return MoveToPosition(_endPosition, crushSpeed);
                _isCrushing = false;

                // Wait at the bottom
                yield return new WaitForSeconds(delayAtBottom);

                // Return up
                yield return MoveToPosition(_startPosition, returnSpeed);
            }
        }

        private IEnumerator MoveToPosition(Vector2 target, float speed)
        {
            while (Vector2.Distance(_rb.position, target) > 0.01f)
            {
                var newPosition = Vector2.MoveTowards(_rb.position, target, speed * Time.fixedDeltaTime);
                _rb.MovePosition(newPosition);
                yield return new WaitForFixedUpdate();
            }
            _rb.MovePosition(target);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_isCrushing) return;
            
            var damageable = collision.collider.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector2 currentStartPosition = Application.isPlaying ? _startPosition : (Vector2)transform.position;
            Vector2 currentEndPosition = currentStartPosition + endPositionOffset;
            Gizmos.DrawLine(currentStartPosition, currentEndPosition);
            Gizmos.DrawWireCube(currentEndPosition, transform.localScale);
        }
    }
}
