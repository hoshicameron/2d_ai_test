using UnityEngine;

namespace PetalsOfHope.Gameplay.LevelElements
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField] private Color gizmoColor = new Color(1, 0, 0, 0.5f);
        [SerializeField] private float gizmoRadius = 0.5f;

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);
        }
    }
}
