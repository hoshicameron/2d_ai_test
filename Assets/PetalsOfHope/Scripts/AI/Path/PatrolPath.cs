using System.Collections.Generic;
using UnityEngine;

namespace PetalsOfHope.AI.Path
{
    /// <summary>
    /// A MonoBehaviour component that defines a series of waypoints for an AI to follow.
    /// This component lives in the scene.
    /// </summary>
    public class PatrolPath : MonoBehaviour
    {
        [Header("Path Configuration")]
        [Tooltip("How close the AI needs to be to a waypoint to consider it 'reached'.")]
        public float StoppingDistance { get; private set; } = 0.2f;
        
        [Tooltip("If true, the AI will loop back to the start after reaching the end.")]
        public bool Loop { get; private set; } = true;

        [field: Tooltip("The list of waypoints in the path, in order.")]
        [field:SerializeField] public List<Transform> Waypoints { get; private set; } = new List<Transform>();

        /// <summary>
        /// Draws the path in the scene editor for easy visualization.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (Waypoints == null || Waypoints.Count < 2)
            {
                return;
            }

            for (int i = 0; i < Waypoints.Count; i++)
            {
                if (Waypoints[i] == null) continue;

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(Waypoints[i].position, 0.3f);

                if (i < Waypoints.Count - 1)
                {
                    if (Waypoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(Waypoints[i].position, Waypoints[i + 1].position);
                    }
                }
                // If looping, draw a line from the last waypoint back to the first.
                else if (Waypoints.Count > 1 && Waypoints[0] != null)
                {
                    Gizmos.DrawLine(Waypoints[i].position, Waypoints[0].position);
                }
            }
        }
    }
}