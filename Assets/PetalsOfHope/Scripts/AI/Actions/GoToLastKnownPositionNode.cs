using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.Scripts.AI.Actions
{
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Go To Last Known Position")]
    public class GoToLastKnownPositionNode : ActionNode
    {
        public override Node Clone() => Instantiate(this);
        protected override void OnStart(AIContext context) { }
        protected override void OnStop(AIContext context) => context.InputSource.SetMoveInput(Vector2.zero);

        protected override NodeState OnUpdate(AIContext context)
        {
            // If we are already close to the last known position, we have succeeded.
            if (Vector2.Distance(context.AgentTransform.position, context.LastKnownPlayerPosition) < 0.5f)
            {
                return NodeState.Success;
            }

            // Move towards the remembered position.
            float directionX = context.LastKnownPlayerPosition.x - context.AgentTransform.position.x;
            context.InputSource.SetMoveInput(new Vector2(Mathf.Sign(directionX), 0));
            return NodeState.Running;
        }
    }
}