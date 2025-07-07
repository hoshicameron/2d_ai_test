using UnityEngine;
using PetalsOfHope.Core.Events;

namespace PetalsOfHope.Gameplay.LevelElements
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private GameEventSO onCollected;
        [SerializeField] private float rotationSpeed = 50f;

        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (onCollected != null)
                {
                    onCollected.Raise();
                }
                // Here you would typically also use an object pool to disable the collectible
                // For now, we'll just destroy it.
                Destroy(gameObject);
            }
        }
    }
}
