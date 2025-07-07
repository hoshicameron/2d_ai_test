using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.Actions
{
    /// <summary>
    /// An action node that moves the agent towards the player's current position.
    /// This node continuously returns Running until it is interrupted by a
    /// higher-priority node.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Chase")]
    public class ChaseNode : ActionNode
    {
        public override Node Clone() => Instantiate(this);

        protected override void OnStart(AIContext context) { }

        /// <summary>
        /// When this node is interrupted (e.g., the player is no longer visible),
        /// ensure the character stops moving.
        /// </summary>
        protected override void OnStop(AIContext context)
        {
            context.InputSource.SetMoveInput(Vector2.zero);
        }

        /// <summary>
        /// Calculates the direction to the player and commands the character to move.
        /// </summary>
        protected override NodeState OnUpdate(AIContext context)
        {
            if (context.PlayerTransform == null)
            {
                // If there's no player, we can't move towards them.
                return NodeState.Failure;
            }
            
            // If this node is running, it means we can see the player.
            // Update the AI's memory of the player's last known position and time.
            context.LastKnownPlayerPosition = context.PlayerTransform.position;
            context.TimeLastSeenPlayer = Time.time;

            // Calculate the horizontal direction to the player.
            Vector2 direction = context.PlayerTransform.position - 
                               context.AgentTransform.position;

            // Normalize the direction to get a value of 1 or -1 (or 0 if directly above/below).
            // This ensures consistent movement speed.
            var horizontalInput = Mathf.Sign(direction.x);
            var verticalInput = Mathf.Sign(direction.y);
            
            if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                context.CurrentFacingDirection = (int)horizontalInput;
            }

            // Command the character to move.
            context.InputSource.SetMoveInput(new Vector2(horizontalInput, verticalInput));
            
            // This action is always "running" until a parent node stops it.
            return NodeState.Running;
        }
    }
}