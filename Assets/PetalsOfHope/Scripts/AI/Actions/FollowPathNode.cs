using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.Scripts.AI.Actions
{
    /// <summary>
    /// An action node that makes the character follow a predefined PatrolPath.
    /// It now reads path-specific properties like stopping distance and looping from the path itself.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Follow Path")]
    public class FollowPathNode : ActionNode
    {
        // This state is unique to each cloned instance of the node.
        [System.NonSerialized]
        private int currentWaypointIndex = 0;

        public override Node Clone()
        {
            FollowPathNode node = Instantiate(this);
            node.currentWaypointIndex = 0; // Reset index for the new clone
            return node;
        }

        protected override void OnStart(AIContext context) 
        {
            // When this node becomes active, we should reset the path progress
            // to ensure the AI always starts from the beginning of the path.
            currentWaypointIndex = 0;
        }

        protected override void OnStop(AIContext context)
        {
            // When this action is interrupted, tell the character to stop moving.
            context.InputSource.SetMoveInput(Vector2.zero);
        }

        protected override NodeState OnUpdate(AIContext context)
        {
            // Check if a path has been assigned.
            if (context.Path == null || context.Path.Waypoints.Count == 0)
            {
                Debug.LogWarning("FollowPathNode is running but no path is assigned in the BehaviorTreeRunner.", context.Character.GameObject);
                return NodeState.Failure;
            }

            // Get the current target waypoint.
            Transform targetWaypoint = context.Path.Waypoints[currentWaypointIndex];
            if (targetWaypoint == null)
            {
                return NodeState.Failure; // Fail if a waypoint is missing.
            }

            Vector2 currentPosition = context.Character.Transform.position;
            Vector2 targetPosition = targetWaypoint.position;

            // Check if we have reached the target waypoint.
            // Read stoppingDistance from the path itself.
            if (Vector2.Distance(currentPosition, targetPosition) < context.Path.StoppingDistance)
            {
                // Increment the index to target the next waypoint.
                currentWaypointIndex++;
                if (currentWaypointIndex >= context.Path.Waypoints.Count)
                {
                    // Read loop property from the path itself.
                    if (context.Path.Loop)
                    {
                        currentWaypointIndex = 0; // Loop back to the start.
                    }
                    else
                    {
                        // Reached the end of a non-looping path.
                        return NodeState.Success;
                    }
                }
            }

            // Calculate the direction to the target and command the character to move.
            Vector2 direction = (targetPosition - currentPosition).normalized;
            context.InputSource.SetMoveInput(direction);

            // This action is always considered "Running" as long as it's active.
            return NodeState.Running;
        }
    }
}