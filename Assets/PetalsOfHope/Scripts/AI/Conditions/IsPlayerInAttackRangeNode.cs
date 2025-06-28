using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.Conditions
{
    /// <summary>
    /// A condition node that checks if the player is within the configured attack range.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Conditions/Is Player In Attack Range")]
    public class IsPlayerInAttackRangeNode : ConditionNode
    {
        protected override NodeState OnUpdate(AIContext context)
        {
            // Fail if the necessary data or references are missing.
            if (context.AttackData == null)
            {
                Debug.LogWarning("AttackData is not assigned in the AIContext.", context.Agent);
                return NodeState.Failure;
            }
            if (context.PlayerTransform == null)
            {
                return NodeState.Failure; // Can't be in range of a player that doesn't exist.
            }

            // Check if the distance to the player is within the attack range defined in the AttackDataSO.
            float distanceToPlayer = Vector2.Distance(context.AgentTransform.position, context.PlayerTransform.position);
            if (distanceToPlayer <= context.AttackData.attackRange)
            {
                return NodeState.Success;
            }

            return NodeState.Failure;
        }

        /// <summary>
        /// Draws a wire sphere gizmo in the editor to visualize the attack range.
        /// </summary>
        public override void DrawGizmos(AIContext context)
        {
            if (context?.AttackData == null) return;

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(context.AgentTransform.position, context.AttackData.attackRange);
        }
    }
}