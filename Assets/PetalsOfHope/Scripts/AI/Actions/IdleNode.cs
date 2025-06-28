using PetalsOfHope.AI.Core;
using UnityEngine;

namespace PetalsOfHope.AI.Actions
{
    /// <summary>
    /// An action node that waits for a duration specified in the IdleDataSO.
    /// It returns Running while the timer is active.
    /// </summary>
    [CreateAssetMenu(menuName = "AI/Behavior Tree/Nodes/Actions/Idle")]
    public class IdleNode : ActionNode
    {
        [System.NonSerialized]
        private float startTime;
        [System.NonSerialized]
        private float duration;

        public override Node Clone() => Instantiate(this);

        /// <summary>
        /// When the node starts, determine which duration to use based on the waitType setting.
        /// </summary>
        protected override void OnStart(AIContext context)
        {
            startTime = Time.time;

            if (context.IdleData == null)
            {
                Debug.LogWarning("WaitNode cannot run because IdleData is not assigned in the AIContext.", context.Agent);
                duration = 0; // Default to 0 if data is missing
                return;
            }

            duration = context.IdleData.initialIdleDuration;
        }

        protected override void OnStop(AIContext context) { }

        protected override NodeState OnUpdate(AIContext context)
        {
            context.InputSource.SetMoveInput(Vector2.zero);
            if (Time.time - startTime > duration)
            {
                return NodeState.Success;
            }
            return NodeState.Running;
        }
    }
}
