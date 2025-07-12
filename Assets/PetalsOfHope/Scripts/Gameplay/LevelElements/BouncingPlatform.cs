using System;
using UnityEngine;

namespace PetalsOfHope.Gameplay.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    public class BouncingPlatform : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The upward force to apply to objects that land on this platform.")]
        private float bounceForce = 20f;

        private Animator _animator;
        private static readonly int Throw = Animator.StringToHash("Throw");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the colliding object has a Rigidbody2D
            var rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                return;
            }

            // We only want to bounce if the collision happens from the top of the platform.
            // To check this, we can find the direction from the platform to the collision point.
            // If that direction is mostly upwards, we can be confident the player is on top.
            bool isCollisionFromTop = false;
            foreach (var contact in collision.contacts)
            {
                // The contact normal points from the other object towards this one.
                // If the normal is pointing mostly down, it means the other object is on top.
                if (contact.normal.y < -0.5f) 
                {
                    isCollisionFromTop = true;
                    break;
                }
            }

            if (isCollisionFromTop)
            {
                _animator.SetTrigger(Throw);
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}
